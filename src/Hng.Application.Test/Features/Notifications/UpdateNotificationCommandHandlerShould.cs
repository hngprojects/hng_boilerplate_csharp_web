using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Handlers;
using Hng.Application.Features.Notifications.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Notifications
{
    public class UpdateNotificationCommandHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly UpdateNotificationCommandHandler _handler;

        public UpdateNotificationCommandHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _handler = new UpdateNotificationCommandHandler(_mockNotificationRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateNotification_WhenNotificationExists()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var command = new UpdateNotificationCommand(notificationId, true);

            var existingNotification = new Notification
            {
                Id = notificationId,
                IsRead = false,
                Message = "Test notification",
                CreatedAt = DateTime.UtcNow
            };

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(existingNotification);

            _mockNotificationRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsRead);
            Assert.Equal(existingNotification.Id, result.Id);
            Assert.Equal(existingNotification.Message, result.Message);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.Is<Notification>(n => n.IsRead == true)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Exactly(2)); // Note: SaveChanges is called twice in the handler
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotificationDoesNotExist()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var command = new UpdateNotificationCommand(notificationId, true);

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync((Notification)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateNotificationToUnread_WhenCommandIsSetToFalse()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var command = new UpdateNotificationCommand(notificationId, false);

            var existingNotification = new Notification
            {
                Id = notificationId,
                IsRead = true,
                Message = "Test notification",
                CreatedAt = DateTime.UtcNow
            };

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(existingNotification);

            _mockNotificationRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsRead);
            Assert.Equal(existingNotification.Id, result.Id);
            Assert.Equal(existingNotification.Message, result.Message);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.Is<Notification>(n => n.IsRead == false)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Exactly(2));
        }
    }
}