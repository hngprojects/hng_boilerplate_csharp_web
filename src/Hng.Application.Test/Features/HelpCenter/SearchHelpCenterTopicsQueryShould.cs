using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Handler;
using Hng.Application.Features.HelpCenter.Queries;
using Hng.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class SearchHelpCenterTopicsQueryShould : HelpCenterTopicsTestsSetup
    {
        private readonly SearchHelpCenterTopicsQueryHandler _handler;

        public SearchHelpCenterTopicsQueryShould()
        {
            _handler = new SearchHelpCenterTopicsQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSearchedHelpCenterTopicsSuccessfully()
        {
            // Arrange
            var searchRequest = new SearchHelpCenterTopicsRequestDto
            {
                Title = "Title",
                Content = "Content"
            };
            var query = new SearchHelpCenterTopicsQuery(searchRequest);

            var topics = new List<HelpCenterTopic>
            {
                new HelpCenterTopic { Id = Guid.NewGuid(), Title = "Title1", Content = "Content1", Author = "Author1" }
            };

            _repositoryMock.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<HelpCenterTopic, bool>>>())).ReturnsAsync(topics);
            _mapperMock.Setup(m => m.Map<List<HelpCenterTopicResponseDto>>(topics)).Returns(new List<HelpCenterTopicResponseDto>
            {
                new HelpCenterTopicResponseDto { Id = topics[0].Id }
            });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Request completed successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
        }

    }

}
