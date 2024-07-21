using Hng.Application.Dto;

namespace Hng.Application.Interfaces
{
    public interface IJobListingService
    {
        Task<JobListingDto> CreateJobListingAsync(CreateJobListingDto createJobListingDto);
    }
}
