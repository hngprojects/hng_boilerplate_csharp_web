using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Application.Features.HelpCenter.Queries;
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
    public class GetHelpCenterTopicByIdQueryShould : HelpCenterTopicsTestsSetup
    {
        private readonly GetHelpCenterTopicByIdQueryHandler _handler;

        public GetHelpCenterTopicByIdQueryShould()
        {
            _handler = new GetHelpCenterTopicByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnHelpCenterTopicByIdSuccessfully()
        {
            // Arrange
            var query = new GetHelpCenterTopicByIdQuery(Guid.NewGuid());

            var existingTopic = new HelpCenterTopic { Id = query.Id, Title = "Title", Content = "Content", Author = "Author" };
            _repositoryMock.Setup(r => r.GetAsync(query.Id)).ReturnsAsync(existingTopic);
            _mapperMock.Setup(m => m.Map<HelpCenterTopicResponseDto>(existingTopic)).Returns(new HelpCenterTopicResponseDto { Id = existingTopic.Id });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Request completed successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(query.Id, result.Data.Id);
        }
    }

}
