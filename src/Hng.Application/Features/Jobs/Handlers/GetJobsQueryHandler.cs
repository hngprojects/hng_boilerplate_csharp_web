using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Jobs.Handlers;

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<JobDto>>
{
    private readonly IRepository<Job> _jobRepository;
    private readonly IMapper _mapper;

    public GetJobsQueryHandler(IMapper mapper, IRepository<Job> jobRepository)
    {
        _mapper = mapper;
        _jobRepository = jobRepository;
    }

    public async Task<IEnumerable<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _jobRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }
}