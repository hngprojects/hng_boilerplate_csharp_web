using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hng.Application.Features.Jobs.Handlers;
using Xunit;
using Hng.Domain.Entities;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Commands;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Test.Features.Job
{
    public class UpdateJobCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Job>> _mockJobRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateJobCommandHandler _handler;

        public UpdateJobCommandShould()
        {
            _mockJobRepository = new Mock<IRepository<Domain.Entities.Job>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateJobCommandHandler(_mockJobRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_JobExists_UpdatesJobSuccessfully()
        {
            // Arrange - Create existing job
            var existingJob = new Domain.Entities.Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Developer",
                Description = "Develop software",
                Location = "Lagos",
                Salary = 30_000,
                Company = "HNG",
                DatePosted = DateTime.Now,
                Level = Domain.Enums.ExperienceLevel.Junior

            };

            var updateJob = new UpdateJobDto
            {
                Title = "Backend Developer",
                Description = "Develop software",
                Level = Domain.Enums.ExperienceLevel.Senior
            };

            var command = new UpdateJobCommand(updateJob, existingJob.Id);

            _mockJobRepository.Setup(repo => repo.GetAsync(existingJob.Id)).ReturnsAsync(existingJob);
            _mockMapper
                .Setup(mapper => mapper.Map(updateJob, existingJob))
                .Callback<UpdateJobDto, Domain.Entities.Job>((src, dest) =>
                 {
                     dest.Title = src.Title;
                     dest.Description = src.Description;
                     dest.Level = src.Level.Value;
                 });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert - Ensure the job was updated and the response is correct
            Assert.NotNull(response.Data);
            Assert.True(response.Success);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(updateJob.Title, existingJob.Title);
            _mockJobRepository.Verify(repo => repo.UpdateAsync(existingJob), Times.Once);
            _mockJobRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_JobDoesNotExist_ReturnsNotFound()
        {
            // Arrange = Simulate job not existing
            var existingJobId = Guid.NewGuid();

            var updateJob = new UpdateJobDto
            {
                Title = "Backend Developer",
                Description = "Develop software",
                Level = Domain.Enums.ExperienceLevel.Senior
            };

            var command = new UpdateJobCommand(updateJob, existingJobId);

            _mockJobRepository.Setup(repo => repo.GetAsync(existingJobId)).ReturnsAsync((Domain.Entities.Job)null);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert - Check response for not found
            Assert.Null(response.Data);
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Job not found", response.Message);
            Assert.Null(response.Data);

        }
    }
}
