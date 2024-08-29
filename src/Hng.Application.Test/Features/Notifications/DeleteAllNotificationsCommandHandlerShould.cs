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
    public class DeleteAllNotificationsCommandHandlerShould
    {
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly DeleteAllNotificationsCommandHandler _handler;

        public DeleteAllNotificationsCommandHandlerShould()
        {
            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new DeleteAllNotificationsCommandHandler(_mockNotificationRepository.Object, _mockAuthenticationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteAllNotifications_WhenNotificationsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteAllNotificationsCommand();

            var notifications = new List<Notification>
            {
                new Notification { Id = Guid.NewGuid(), UserId = userId },
                new Notification { Id = Guid.NewGuid(), UserId = userId }
            };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(notifications);

            _mockNotificationRepository.Setup(r => r.DeleteAsync(It.IsAny<Notification>()));

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockNotificationRepository.Verify(r => r.DeleteAsync(It.IsAny<Notification>()), Times.Exactly(notifications.Count));
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenNoNotificationsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteAllNotificationsCommand();

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockNotificationRepository.Verify(r => r.DeleteAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}