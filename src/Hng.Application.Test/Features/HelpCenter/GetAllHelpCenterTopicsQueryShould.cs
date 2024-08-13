using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Application.Features.HelpCenter.Queries;
using Hng.Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class GetAllHelpCenterTopicsQueryShould : HelpCenterTopicsTestsSetup
    {
        private readonly GetHelpCenterTopicsQueryHandler _handler;

        public GetAllHelpCenterTopicsQueryShould()
        {
            _handler = new GetHelpCenterTopicsQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllHelpCenterTopicsSuccessfully()
        {
            // Arrange
            var query = new GetHelpCenterTopicsQuery();

            var topics = new List<HelpCenterTopic>
        {
            new HelpCenterTopic { Id = Guid.NewGuid(), Title = "Title1", Content = "Content1", Author = "Author1" },
            new HelpCenterTopic { Id = Guid.NewGuid(), Title = "Title2", Content = "Content2", Author = "Author2" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<HelpCenterTopic, object>>[]>())).ReturnsAsync(topics);
            _mapperMock.Setup(m => m.Map<List<HelpCenterTopicResponseDto>>(topics)).Returns(new List<HelpCenterTopicResponseDto>
        {
            new HelpCenterTopicResponseDto { Id = topics[0].Id },
            new HelpCenterTopicResponseDto { Id = topics[1].Id }
        });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Request completed successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }
    }

}
