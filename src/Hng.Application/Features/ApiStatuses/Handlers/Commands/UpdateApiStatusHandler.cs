using CSharpFunctionalExtensions;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Hng.Application.Features.ApiStatuses.Handlers.Commands
{
    public class UpdateApiStatusHandler : IRequestHandler<UpdateApiStatusDto, Result<CreateApiStatusResponseDto>>
    {
        private readonly IRepository<ApiStatus> _apistatusRepository;

        public UpdateApiStatusHandler(IRepository<ApiStatus> apistatusRepository) => _apistatusRepository = apistatusRepository;

        public async Task<Result<CreateApiStatusResponseDto>> Handle(UpdateApiStatusDto request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Report.Length == 0
                || (request.Report.Length != 0 && request.Report.ContentType != "application/json"))
                {
                    return Result.Failure<CreateApiStatusResponseDto>("Report can only be json format");
                }

                using var stream = request.Report.OpenReadStream();
                var jsonObject = await JsonSerializer.DeserializeAsync<ApiStatusResponseModel>(stream, cancellationToken: cancellationToken);

                var groupCollection = jsonObject.collection.item
                .GroupBy(c => c.name)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().item.Select(x => x.id).ToList()
                );

                var failedCollections = jsonObject.run.failures
                    .Select(x => x.parent.id)
                    .ToHashSet();  // Use HashSet for O(1) lookup

                var apiStatuses = groupCollection.Select(item =>
                {
                    var executionsForGroup = jsonObject.run.executions
                        .Where(e => e.item.request.url.path.Contains(item.Key));

                    var responseCount = executionsForGroup.Count();
                    var averageResponseTime = responseCount > 0
                        ? executionsForGroup.Sum(x => x.response.responseTime) / responseCount
                        : 0;

                    var status = item.Value.Any(failedCollections.Contains)
                        ? ApiStatusType.Degraded
                        : ApiStatusType.Operational;

                    return new ApiStatus
                    {
                        ApiGroup = item.Key,
                        Status = status,
                        Details = status == ApiStatusType.Operational ? "All tests passed" : "Some tests failed",
                        ResponseTime = averageResponseTime,
                    };
                }).ToList();

                await _apistatusRepository.AddRangeAsync(apiStatuses);
                await _apistatusRepository.SaveChanges();

                return Result.Success(new CreateApiStatusResponseDto()
                {
                    Message = "success",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
