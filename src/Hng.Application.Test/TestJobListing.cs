using Hng.Web.Controllers;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Xunit;

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
        }

        private void SetUpAuthorizedController()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var httpContext = new DefaultHttpContext();
            httpContext.User = user;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            _controller.ControllerContext = controllerContext;
        }

        private void SetUpUnauthorizedController()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            _controller.ControllerContext = controllerContext;
        }

        [Fact]
        public async Task CreateJob_ReturnsCreatedAtActionResult_WithJobListing()
        {
            // Arrange
            SetUpAuthorizedController();
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
            SetUpAuthorizedController();
            var invalidDto = new CreateJobListingDto(); // Empty DTO

            // Act
            var result = await _controller.CreateJobListing(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task InvalidMethod_ReturnsMethodNotAllowed()
        {
            // Arrange
            SetUpAuthorizedController();
            _controller.ControllerContext.HttpContext.Request.Method = "DELETE";

            // Act
            var result = await _controller.CreateJobListing(new CreateJobListingDto());

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(405, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateJob_WithoutAuthorization_ReturnsUnauthorized()
        {
            // Arrange
            SetUpUnauthorizedController();
            var createDto = new CreateJobListingDto
            {
                Title = "Software Developer",
                Description = "We are looking for a talented software developer...",
                Location = "New York, NY",
                Salary = "$80,000 - $120,000",
                JobType = "Full-time",
                CompanyName = "Tech Innovations Inc."
            };

            // Act
            var result = await _controller.CreateJobListing(createDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result.Result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
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
