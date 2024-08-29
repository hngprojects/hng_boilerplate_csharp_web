using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Domain.Entities;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class UpdateHelpCenterTopicCommandShould : HelpCenterTopicsTestsSetup
    {
        private readonly UpdateHelpCenterTopicCommandHandler _handler;

        public UpdateHelpCenterTopicCommandShould()
        {
            _handler = new UpdateHelpCenterTopicCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateHelpCenterTopicSuccessfully()
        {
            // Arrange
            var updateRequest = new UpdateHelpCenterTopicRequestDto { Title = "Updated Title", Content = "Updated Content", Author = "Updated Author" };
            var command = new UpdateHelpCenterTopicCommand(Guid.NewGuid(), updateRequest);

            var existingTopic = new HelpCenterTopic { Id = command.Id, Title = "Title", Content = "Content", Author = "Author" };
            _repositoryMock.Setup(r => r.GetAsync(command.Id)).ReturnsAsync(existingTopic);

            // Ensure the mapping correctly maps the update request to the existing topic
            _mapperMock.Setup(m => m.Map(command.Request, existingTopic)).Returns(existingTopic);

            // Ensure the update method is called
            _repositoryMock.Setup(r => r.UpdateAsync(existingTopic)).Verifiable();

            // Ensure the response mapping is correct
            _mapperMock.Setup(m => m.Map<HelpCenterTopicResponseDto>(existingTopic)).Returns(new HelpCenterTopicResponseDto { Id = existingTopic.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Topic updated successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(command.Id, result.Data.Id);

            // Verify that update was called
            _repositoryMock.Verify(r => r.UpdateAsync(existingTopic), Times.Once);
        }

    }

}

