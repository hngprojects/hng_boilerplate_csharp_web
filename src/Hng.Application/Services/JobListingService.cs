using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Application.Services
{
    public class JobListingService : IJobListingService
    {
        private readonly IGenericRepository<JobListing> _jobListingRepository;
        private readonly IMapper _mapper;

        public JobListingService(IGenericRepository<JobListing> jobListingRepository, IMapper mapper)
        {
            _jobListingRepository = jobListingRepository;
            _mapper = mapper;
        }

        public async Task<JobListingDto> CreateJobListingAsync(CreateJobListingDto createJobListingDto)
        {
            var jobListing = _mapper.Map<JobListing>(createJobListingDto);
            jobListing.Id = Guid.NewGuid();
            var createdJobListing = await _jobListingRepository.AddAsync(jobListing);
            return _mapper.Map<JobListingDto>(createdJobListing);
        }
    }
}
