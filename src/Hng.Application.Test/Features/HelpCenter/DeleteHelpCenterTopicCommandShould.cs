using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class DeleteHelpCenterTopicCommandShould : HelpCenterTopicsTestsSetup
    {
        private readonly DeleteHelpCenterTopicCommandHandler _handler;

        public DeleteHelpCenterTopicCommandShould()
        {
            _handler = new DeleteHelpCenterTopicCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteHelpCenterTopicSuccessfully()
        {
            // Arrange
            var command = new DeleteHelpCenterTopicCommand(Guid.NewGuid());

            var existingTopic = new HelpCenterTopic { Id = command.Id, Title = "Title", Content = "Content", Author = "Author" };
            _repositoryMock.Setup(r => r.GetAsync(command.Id)).ReturnsAsync(existingTopic);
            _repositoryMock.Setup(r => r.DeleteAsync(existingTopic)).ReturnsAsync(existingTopic);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Topic deleted successfully", result.Message);
        }
    }

}
