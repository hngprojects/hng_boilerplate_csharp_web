using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Jobs.Handlers;

public class GetJobByIdQueryHandler(IRepository<Job> jobRepository, IMapper mapper) : IRequestHandler<GetJobByIdQuery, JobDto>
{
    public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await jobRepository.GetBySpec(j => j.Id == request.JobId);

        return job == null ? null : mapper.Map<JobDto>(job);
    }
}