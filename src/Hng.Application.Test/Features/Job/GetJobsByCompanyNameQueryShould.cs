using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Handlers;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Job
{
    public class GetJobsByCompanyNameQueryShould
    {
        private readonly Mock<IRepository<Domain.Entities.Job>> _mockRepository;
        private readonly IMapper _mapper;
        private readonly GetJobsByCompanyNameQueryHandler _handler;

        public GetJobsByCompanyNameQueryShould()
        {
            _mockRepository = new Mock<IRepository<Domain.Entities.Job>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.Job, JobDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetJobsByCompanyNameQueryHandler(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task ReturnOnlyJobsForCompany_WhenCompanyNameIsProvided()
        {
            // Arrange
            var jobs = new List<Domain.Entities.Job>
            {
                new Domain.Entities.Job { Id = Guid.NewGuid(), Title = "Software Engineer", Company = "TechCorp" },
                new Domain.Entities.Job { Id = Guid.NewGuid(), Title = "Frontend Developer", Company = "TechCorp" },
                new Domain.Entities.Job { Id = Guid.NewGuid(), Title = "Backend Developer", Company = "OtherCorp" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(jobs);

            var query = new GetJobsByCompanyNameQuery("TechCorp");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Jobs retrieved successfully.", result.Message);
            Assert.Equal(2, result.Data.Count); // Only 2 jobs should match "TechCorp"
            Assert.All(result.Data, job => Assert.Equal("TechCorp", job.Company)); // Ensure all jobs are from TechCorp
        }

        [Fact]
        public async Task ReturnBadRequest_WhenCompanyNameIsEmpty()
        {
            // Arrange
            var query = new GetJobsByCompanyNameQuery(""); // Empty company name

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal("Company name must be provided.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task ReturnNotFound_WhenNoJobsExistForCompany()
        {
            // Arrange
            var jobs = new List<Domain.Entities.Job>
            {
                new Domain.Entities.Job { Id = Guid.NewGuid(), Title = "Software Engineer", Company = "OtherCorp" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(jobs);

            var query = new GetJobsByCompanyNameQuery("NonExistentCorp");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal("No jobs were found for the specified company.", result.Message);
            Assert.Null(result.Data);
        }
    }
}
