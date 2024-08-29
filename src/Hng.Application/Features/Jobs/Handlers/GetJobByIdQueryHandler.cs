using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Jobs.Handlers;

public class GetJobByIdQueryHandler(IRepository<Job> jobRepository, IMapper mapper) : IRequestHandler<GetJobByIdQuery, GetJobRequestDto>
{
    public async Task<GetJobRequestDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await jobRepository.GetBySpec(j => j.Id == request.JobId);

        if (job is null)
        {
            return new GetJobRequestDto()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Job Not Found",
            };
        }

        var jobDto = mapper.Map<JobDto>(job);

        return new GetJobRequestDto()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Job successfully retrieved",
            Data = jobDto
        };
    }
}