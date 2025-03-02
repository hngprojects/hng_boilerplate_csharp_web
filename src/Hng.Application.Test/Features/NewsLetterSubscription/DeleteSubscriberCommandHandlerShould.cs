using Hng.Application.Features.NewsLetterSubscription.Commands;
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
            var subscriberId = Guid.NewGuid();
            var subscriber = new NewsLetterSubscriber { Id = subscriberId };
            var command = new DeleteSubscriberCommand(subscriberId);

            _mockRepository.Setup(r => r.GetAsync(subscriberId))
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

            _mockRepository.Verify(r => r.GetAsync(subscriberId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(subscriber), Times.Once);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenSubscriberDoesNotExist()
        {
            var subscriberId = Guid.NewGuid();
            var command = new DeleteSubscriberCommand(subscriberId);

            _mockRepository.Setup(r => r.GetAsync(subscriberId))
                .ReturnsAsync((NewsLetterSubscriber)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Subscriber not found.", result.Message);

            _mockRepository.Verify(r => r.GetAsync(subscriberId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
