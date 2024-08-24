using AutoMapper;
using Hng.Application.Features.ApiStatuses.Commands;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Handlers.Commands;
using Hng.Application.Features.ApiStatuses.Mappers;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Xunit;

public class CreateApiStatusCommandShould
{
    private readonly Mock<IRepository<ApiStatus>> _mockRepository;
    private readonly IMapper _mapper;
    private readonly CreateApiStatusCommandHandler _handler;

    public CreateApiStatusCommandShould()
    {
        _mockRepository = new Mock<IRepository<ApiStatus>>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApiStatusesMapperProfile>();
        });

        _mapper = configuration.CreateMapper();

        _handler = new CreateApiStatusCommandHandler(_mockRepository.Object, _mapper);
    }

    [Fact]
    public async Task ReturnBadRequestForInvalidFileType()
    {
        // Arrange
        var file = new FormFile(new MemoryStream(new byte[0]), 0, 0, "file", "test.txt");
        var command = new CreateApiStatusCommand { File = file };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid file type. Only JSON files are allowed.", result.Message);
    }

    [Fact]
    public async Task ReturnBadRequestForInvalidJsonStructure()
    {
        // Arrange
        var json = "{ }"; // Invalid JSON structure
        var file = new FormFile(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)), 0, json.Length, "file", "invalid.json");
        var command = new CreateApiStatusCommand { File = file };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid or empty JSON structure.", result.Message);
    }

    [Fact]
    public async Task SaveApiStatusesSuccessfully()
    {
        // Arrange
        var apiStatusModels = new List<ApiStatusModel>
        {
            new ApiStatusModel { ApiGroup = "Group1", Status = ApiStatusType.Operational, ResponseTime = 100, Details = "All good" }
        };
        var apiStatusWrapper = new ApiStatusWrapper
        {
            Collection = new Dictionary<string, List<ApiStatusModel>>
            {
                { "key", apiStatusModels }
            }
        };

        var json = JsonConvert.SerializeObject(apiStatusWrapper);
        var file = new FormFile(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)), 0, json.Length, "file", "valid.json");
        var command = new CreateApiStatusCommand { File = file };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<List<ApiStatus>>()), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal("API statuses created successfully.", result.Message);
    }
}