using AutoMapper;
using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Jobs.Handlers;

public class CreateJobCommandHandler(IRepository<Job> jobRepository, IMapper mapper)
    : IRequestHandler<CreateJobCommand, JobDto>
{
    public async Task<JobDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = mapper.Map<Job>(request.JobBody);
        job.DatePosted = DateTime.UtcNow;

        await jobRepository.AddAsync(job);
        await jobRepository.SaveChanges();
        return mapper.Map<JobDto>(job);
    }
}