using AutoMapper;
using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Handlers;
using Hng.Application.Features.Jobs.Mappers;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Job;

public class CreateJobShouldTest
{
    private readonly IMapper _mapper;
    private readonly Mock<IRepository<Domain.Entities.Job>> _repositoryMock;
    private readonly CreateJobCommandHandler _handler;

    public CreateJobShouldTest()
    {
        var mappingProfile = new JobMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        _mapper = new Mapper(configuration);

        _repositoryMock = new Mock<IRepository<Domain.Entities.Job>>();
        _handler = new CreateJobCommandHandler(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedJob()
    {
        var expectedId = Guid.NewGuid();
        var createDto = new CreateJobDto
        {
            Title = "Test Job",
            Description = "A test organization",
            Location = "Sheffield",
            Salary = 1000.00,
            Level = (ExperienceLevel)1,
            Company = "Meta"
        };
        
        _repositoryMock.Setup(j => j.AddAsync(It.IsAny<Domain.Entities.Job>()))
            .ReturnsAsync((Domain.Entities.Job job) =>
            {
                job.Id = expectedId;
                return job;
            });
        
        var command = new CreateJobCommand(createDto);

        var result = await _handler.Handle(command, default);
        
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.Description, result.Description);
        Assert.Equal(createDto.Location, result.Location);
        Assert.Equal(createDto.Salary, result.Salary);
        Assert.Equal((ExperienceLevel)createDto.Level, result.Level);
        Assert.Equal(createDto.Company, result.Company);
    }
}