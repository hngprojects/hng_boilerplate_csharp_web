using AutoMapper;
using Hng.Application.Features.ApiStatuses.Commands;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.ApiStatuses.Handlers.Commands
{
    public class CreateApiStatusCommandHandler : IRequestHandler<CreateApiStatusCommand, CreateApiStatusResponseDto>
    {
        private readonly IRepository<ApiStatus> _apiStatusRepository;
        private readonly IMapper _mapper;

        public CreateApiStatusCommandHandler(IRepository<ApiStatus> apiStatusRepository, IMapper mapper)
        {
            _apiStatusRepository = apiStatusRepository;
            _mapper = mapper;
        }

        public async Task<CreateApiStatusResponseDto> Handle(CreateApiStatusCommand request, CancellationToken cancellationToken)
        {
            // Validate the file type
            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            if (fileExtension != ".json")
            {
                return new CreateApiStatusResponseDto
                {
                    Message = "Invalid file type. Only JSON files are allowed.",
                    StatusCode = 400
                };
            }

            // Read the JSON file
            ApiStatusWrapper wrapper;
            using (var stream = request.File.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                var content = await reader.ReadToEndAsync();
                wrapper = JsonConvert.DeserializeObject<ApiStatusWrapper>(content);
            }

            if (wrapper == null || wrapper.Collection == null || !wrapper.Collection.Any())
            {
                return new CreateApiStatusResponseDto
                {
                    Message = "Invalid or empty JSON structure.",
                    StatusCode = 400
                };
            }

            // Flatten the dictionary into a single list
            var apiStatusModels = wrapper.Collection.SelectMany(kvp => kvp.Value).ToList();

            // Map to domain entities and save to the database
            var apiStatuses = _mapper.Map<List<ApiStatus>>(apiStatusModels);
            await _apiStatusRepository.AddRangeAsync(apiStatuses);
            await _apiStatusRepository.SaveChanges();

            return new CreateApiStatusResponseDto
            {
                Message = "API statuses created successfully.",
                StatusCode = 201
            };
        }
    }
}