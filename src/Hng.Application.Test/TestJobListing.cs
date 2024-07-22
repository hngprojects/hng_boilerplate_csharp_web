using Hng.Web.Controllers;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Hng.Application.Test
{
    public class JobsControllerTests
    {
        private readonly StubJobListingService _stubJobListingService;
        private readonly JobsController _controller;

        public JobsControllerTests()
        {
            _stubJobListingService = new StubJobListingService();
            _controller = new JobsController(_stubJobListingService);
            
            // Set up the controller with routing information
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Jobs";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                RouteData = routeData
            };
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

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            var returnValue = Assert.IsType<JobListingDto>(createdAtActionResult.Value);
            Assert.Equal(createdDto.Id, returnValue.Id);
            Assert.Equal(createDto.Title, returnValue.Title);
        }

        [Fact]
        public async Task CreateJob_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidDto = new CreateJobListingDto(); // Empty DTO

            // Act
            var result = await _controller.CreateJobListing(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void InvalidMethod_ReturnsMethodNotAllowed()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "DELETE";
            httpContext.Request.Path = "/api/v1/jobs";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            _controller.ControllerContext = controllerContext;

            // Act
            var result = _controller.CreateJobListing(new CreateJobListingDto());

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(405, statusCodeResult.StatusCode);
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
}
