using AutoMapper;
using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Jobs.Handlers;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, JobDto>
{
    
    private readonly IRepository<Job> _jobRepository;
    private readonly IMapper _mapper;

    public CreateJobCommandHandler(IRepository<Job> jobRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
    }
    public async Task<JobDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = _mapper.Map<Job>(request.JobBody);
        job.DatePosted = DateTime.UtcNow;

        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChanges();
        return _mapper.Map<JobDto>(job);
    }
}