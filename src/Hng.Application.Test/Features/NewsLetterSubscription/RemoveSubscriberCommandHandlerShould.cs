using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.NewsLetterSubscription
{
    public class DeleteSubscriberByEmailHandlerShould
    {
        private readonly Mock<IRepository<NewsLetterSubscriber>> _mockRepository;
        private readonly DeleteSubscriberByEmailHandler _handler;

        public DeleteSubscriberByEmailHandlerShould()
        {
            _mockRepository = new Mock<IRepository<NewsLetterSubscriber>>();
            _handler = new DeleteSubscriberByEmailHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteSubscriber_WhenSubscriberExists()
        {
            // Arrange
            var email = "example@test.com";
            var dto = new NewsLetterSubscriptionDto { Email = email };
            var subscriber = new NewsLetterSubscriber { Email = email, IsDeleted = false };
            var command = new DeleteSubscriberByEmailCommand(dto);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()))
                .ReturnsAsync(subscriber);

            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<NewsLetterSubscriber>()))
                .Returns(Task.CompletedTask);

            _mockRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Subscriber deleted successfully.", result.Message);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(subscriber), Times.Once);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@test.com";
            var dto = new NewsLetterSubscriptionDto { Email = email };
            var command = new DeleteSubscriberByEmailCommand(dto);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()))
                .ReturnsAsync((NewsLetterSubscriber)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Subscriber not found.", result.Message);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenSubscriberAlreadyDeleted()
        {
            // Arrange
            var email = "deleted@test.com";
            var dto = new NewsLetterSubscriptionDto { Email = email };
            var subscriber = new NewsLetterSubscriber { Email = email, IsDeleted = true };
            var command = new DeleteSubscriberByEmailCommand(dto);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()))
                .ReturnsAsync(subscriber);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Subscriber already deleted.", result.Message);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}