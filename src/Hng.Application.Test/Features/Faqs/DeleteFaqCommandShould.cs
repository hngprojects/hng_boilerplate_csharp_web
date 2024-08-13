using Hng.Application.Features.Faq.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Faqs
{
    public class DeleteFaqCommandShould
    {
        private readonly Mock<IRepository<Faq>> _repositoryMock;
        private readonly DeleteFaqCommandHandler _handler;

        public DeleteFaqCommandShould()
        {
            _repositoryMock = new Mock<IRepository<Faq>>();
            _handler = new DeleteFaqCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteFaq_WhenFaqExists()
        {
            // Arrange
            var faqId = Guid.NewGuid();
            var faq = new Faq { Id = faqId, Question = "Test Question", Answer = "Test Answer" };

            _repositoryMock.Setup(x => x.GetAsync(faqId)).ReturnsAsync(faq);

            var command = new DeleteFaqCommand(faqId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(x => x.DeleteAsync(faq), Times.Once);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("FAQ deleted successfully", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenFaqDoesNotExist()
        {
            // Arrange
            var faqId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.GetAsync(faqId)).ReturnsAsync((Faq)null);

            var command = new DeleteFaqCommand(faqId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("FAQ not found", result.Message);
        }
    }

}
