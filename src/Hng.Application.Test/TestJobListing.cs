using Hng.Web.Controllers;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hng.Api.Tests.Controllers
{
    public class JobsControllerTests
    {
        private readonly StubJobListingService _stubJobListingService;
        private readonly JobsController _controller;

        public JobsControllerTests()
        {
            _stubJobListingService = new StubJobListingService();
            _controller = new JobsController(_stubJobListingService);
        }

        [Fact]
        public async Task CreateJob_ReturnsCreatedAtActionResult_WithJobListing()
        {
            // Arrange
            var createDto = new CreateJobListingDto
            {
                Title = "Software Developer",
                Description = "We are looking for a talented software developer...",
                Location = "New York, NY",
                Salary = "$80,000 - $120,000",
                JobType = "Full-time",
                CompanyName = "Tech Innovations Inc."
            };

            var createdDto = new JobListingDto
            {
                Id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                Location = createDto.Location,
                Salary = createDto.Salary,
                JobType = createDto.JobType,
                CompanyName = createDto.CompanyName,
                CreatedAt = DateTime.UtcNow
            };

            _stubJobListingService.CreateJobListingAsyncImpl = _ => Task.FromResult(createdDto);

            // Act
            var result = await _controller.CreateJobListing(createDto);
            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<JobListingDto>(createdAtActionResult.Value);
            Assert.Equal(createdDto.Id, returnValue.Id);
            Assert.Equal(createDto.Title, returnValue.Title);
        }
    }
}


public class StubJobListingService : IJobListingService
{
    public Func<CreateJobListingDto, Task<JobListingDto>> CreateJobListingAsyncImpl { get; set; }

    public Task<JobListingDto> CreateJobListingAsync(CreateJobListingDto createJobListingDto)
    {
        return CreateJobListingAsyncImpl(createJobListingDto);
    }
}
