using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Handlers;
using Hng.Application.Features.Notifications.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Notifications
{
    public class MarkAllCommandHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly MarkAllCommandHandler _handler;

        public MarkAllCommandHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new MarkAllCommandHandler(_mockNotificationRepository.Object, _mockAuthenticationService.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldMarkAllNotificationsAsRead_WhenNotificationsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new MarkAllCommand(true);

            var notifications = new List<Notification>
            {
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "Notification 1", CreatedAt = DateTime.UtcNow },
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "Notification 2", CreatedAt = DateTime.UtcNow }
            };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(notifications);

            _mockNotificationRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notifications.Count, result.Count);
            Assert.True(result.All(n => n.IsRead));

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Exactly(notifications.Count));
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoNotificationsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new MarkAllCommand(true);

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldMarkAllNotificationsAsUnread_WhenCommandIsSetToFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new MarkAllCommand(false);

            var notifications = new List<Notification>
            {
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "Notification 1", CreatedAt = DateTime.UtcNow },
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "Notification 2", CreatedAt = DateTime.UtcNow }
            };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(notifications);

            _mockNotificationRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notifications.Count, result.Count);
            Assert.True(result.All(n => !n.IsRead));

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Exactly(notifications.Count));
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}