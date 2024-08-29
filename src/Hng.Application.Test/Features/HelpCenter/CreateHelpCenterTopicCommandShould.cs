using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Domain.Entities;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class CreateHelpCenterTopicCommandShould : HelpCenterTopicsTestsSetup
    {
        private readonly CreateHelpCenterTopicCommandHandler _handler;

        public CreateHelpCenterTopicCommandShould()
        {
            _handler = new CreateHelpCenterTopicCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateHelpCenterTopicSuccessfully()
        {
            // Arrange
            var createRequest = new CreateHelpCenterTopicRequestDto { Title = "Title", Content = "Content" };
            var command = new CreateHelpCenterTopicCommand(createRequest);

            var helpCenterTopic = new HelpCenterTopic { Id = Guid.NewGuid(), Title = "Title", Content = "Content", Author = "Author" };
            _mapperMock.Setup(m => m.Map<HelpCenterTopic>(command)).Returns(helpCenterTopic);
            _repositoryMock.Setup(r => r.AddAsync(helpCenterTopic)).ReturnsAsync(helpCenterTopic);
            _mapperMock.Setup(m => m.Map<HelpCenterTopicResponseDto>(helpCenterTopic)).Returns(new HelpCenterTopicResponseDto { Id = helpCenterTopic.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Help Center Topic created successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(helpCenterTopic.Id, result.Data.Id);
        }

    }

}
