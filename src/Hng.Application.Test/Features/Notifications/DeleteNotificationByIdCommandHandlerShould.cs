using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Notifications
{
    public class DeleteNotificationByIdCommandHandlerShould
    {
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly DeleteNotificationByIdCommandHandler _handler;

        public DeleteNotificationByIdCommandHandlerShould()
        {
            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new DeleteNotificationByIdCommandHandler(_mockNotificationRepository.Object, _mockAuthenticationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteNotification_WhenNotificationExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var command = new DeleteNotificationByIdCommand(notificationId);

            var notification = new Notification { Id = notificationId, UserId = userId };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(notification);

            _mockNotificationRepository.Setup(r => r.DeleteAsync(It.IsAny<Notification>()));

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockNotificationRepository.Verify(r => r.DeleteAsync(It.Is<Notification>(n => n.Id == notificationId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenNotificationDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var command = new DeleteNotificationByIdCommand(notificationId);

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync((Notification)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockNotificationRepository.Verify(r => r.DeleteAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}