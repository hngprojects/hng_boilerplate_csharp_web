using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Handlers;
using Hng.Application.Features.Jobs.Mappers;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Job;

public class GetJobsQueryShould
{
    private readonly Mock<IRepository<Domain.Entities.Job>> _jobRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetJobsQueryHandler _handler;

    public GetJobsQueryShould()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<JobMapperProfile>());
        _mapper = config.CreateMapper();

        _jobRepositoryMock = new Mock<IRepository<Domain.Entities.Job>>();

        _handler = new GetJobsQueryHandler(_mapper, _jobRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedJobs()
    {
        var jobs = new List<Domain.Entities.Job>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Software Developer",
                Description = "Develop software applications",
                Location = "New York",
                Salary = 100000,
                Level = ExperienceLevel.Intermediate,
                Company = "Tech Corp",
                DatePosted = DateTime.Now.AddDays(-5)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Data Analyst",
                Description = "Analyze data and create reports",
                Location = "San Francisco",
                Salary = 90000,
                Level = ExperienceLevel.Senior,
                Company = "Data Inc",
                DatePosted = DateTime.Now.AddDays(-2)
            }
        };
        
        _jobRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(jobs);

        var query = new GetJobsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var jobDtos = Assert.IsAssignableFrom<IEnumerable<JobDto>>(result);
        var enumerable = jobDtos.ToList();
        {
            Assert.Equal(2, enumerable.Count);

            var collection = result.ToList();
            Assert.Contains(collection,
                job => job.Title == "Software Developer" && job.Level == ExperienceLevel.Intermediate);
            Assert.Contains(collection, job => job.Title == "Data Analyst" && job.Level == ExperienceLevel.Senior);

            var firstJob = enumerable.First();
            Assert.Equal("Software Developer", firstJob.Title);
            Assert.Equal("Develop software applications", firstJob.Description);
            Assert.Equal("New York", firstJob.Location);
            Assert.Equal(100000, firstJob.Salary);
            Assert.Equal("Tech Corp", firstJob.Company);
            Assert.Equal(jobs[0].DatePosted, firstJob.DatePosted);

            var secondJob = enumerable.Last();
            Assert.Equal("Data Analyst", secondJob.Title);
            Assert.Equal("Analyze data and create reports", secondJob.Description);
            Assert.Equal("San Francisco", secondJob.Location);
            Assert.Equal(90000, secondJob.Salary);
            Assert.Equal("Data Inc", secondJob.Company);
            Assert.Equal(jobs[1].DatePosted, secondJob.DatePosted);
        }

        _jobRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}