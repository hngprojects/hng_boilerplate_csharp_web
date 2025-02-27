using System.Linq.Expressions;
using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.NewsLetterSubscription
{
    public class DeleteSubscriberHandlerShould
    {
        private readonly Mock<IRepository<NewsLetterSubscriber>> _mockRepository;
        private readonly DeleteSubscriberHandler _handler;

        public DeleteSubscriberHandlerShould()
        {
            _mockRepository = new Mock<IRepository<NewsLetterSubscriber>>();
            _handler = new DeleteSubscriberHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteSubscriber_WhenSubscriberExists()
        {
            var email = "test@example.com";
            var subscriber = new NewsLetterSubscriber { Email = email };

            var subscriptionDto = new NewsLetterSubscriptionDto { Email = email };
            var command = new DeleteSubscriberCommand(subscriptionDto);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()))
                .ReturnsAsync(subscriber);

            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<NewsLetterSubscriber>()))
                .ReturnsAsync((NewsLetterSubscriber subscriber) => subscriber);

            _mockRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Subscriber deleted successfully.", result.Message);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(subscriber), Times.Once);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var subscriptionDto = new NewsLetterSubscriptionDto { Email = email };
            var command = new DeleteSubscriberCommand(subscriptionDto);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()))
                .ReturnsAsync((NewsLetterSubscriber)null); // subscriber not found

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Subscriber not found.", result.Message);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenEmailIsInvalid()
        {
            // Arrange
            var subscriptionDto = new NewsLetterSubscriptionDto { Email = "" };
            var command = new DeleteSubscriberCommand(subscriptionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(400, result.StatusCode);

            _mockRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NewsLetterSubscriber, bool>>>()), Times.Never);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}