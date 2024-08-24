using AutoMapper;
using Hng.Application.Features.ApiStatuses.Commands;
using Hng.Application.Features.ApiStatuses.Handlers.Commands;
using Hng.Application.Features.ApiStatuses.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.ApiStatuses
{
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
        public async Task Handle_ShouldReturnBadRequest_ForInvalidFileType()
        {
            // Arrange
            var command = new CreateApiStatusCommand
            {
                File = new FormFile(Stream.Null, 0, 0, "Data", "test.png") // Invalid file type
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid file type. Only JSON files are allowed.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldSaveApiStatuses_ForValidJsonFile()
        {
            // Arrange
            var json = "[{\"api_group\":\"Group1\",\"status\":0,\"response_time\":123,\"details\":\"All good\"}]";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(json);
            writer.Flush();
            stream.Position = 0;

            var command = new CreateApiStatusCommand
            {
                File = new FormFile(stream, 0, stream.Length, "Data", "test.json")
            };

            _mockRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<ApiStatus>>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(x => x.AddRangeAsync(It.IsAny<List<ApiStatus>>()), Times.Once);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("API statuses created successfully.", result.Message);
        }
    }
}