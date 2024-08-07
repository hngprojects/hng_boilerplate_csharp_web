using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
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
    public class CreateNotificationCommandHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<NotificationSettings>> _mockNotificationSettingsRepository;
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly CreateNotificationCommandHandler _handler;

        public CreateNotificationCommandHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationSettingsRepository = new Mock<IRepository<NotificationSettings>>();
            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new CreateNotificationCommandHandler(
                _mockNotificationSettingsRepository.Object,
                _mockNotificationRepository.Object,
                _mockUserRepository.Object,
                _mockAuthenticationService.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateNewNotificationForUserWithSettings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto { Message = "Test message" };
            var command = new CreateNotificationCommand(createNotificationDto);

            var user = new User { Id = userId };
            var settings = new NotificationSettings
            {
                UserId = userId,
                EmailNotifications = true,
                ActivityWorkspaceEmail = true
            };
            var newNotification = new Notification
            {
                UserId = userId,
                Message = command.Notification.Message
            };

            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
              .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _mockNotificationSettingsRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync(settings);
            _mockNotificationRepository.Setup(r => r.AddAsync(It.IsAny<Notification>()))
                .ReturnsAsync(newNotification);
            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Notification);
            Assert.Equal(command.Notification.Message, result.Notification.Message);
            Assert.Equal(userId, result.Notification.UserId);

            _mockNotificationRepository.Verify(r => r.AddAsync(It.Is<Notification>(n => n.UserId == userId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotCreateNotificationIfSettingsDoNotAllow()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto { Message = "Test message" };
            var command = new CreateNotificationCommand(createNotificationDto);

            var user = new User { Id = userId };
            var settings = new NotificationSettings
            {
                UserId = userId,
                EmailNotifications = false,
                ActivityWorkspaceEmail = false
            };

            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
               .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _mockNotificationSettingsRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync(settings);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Notification);
            Assert.Equal("Notification settings do not allow this action", result.FailureResponse.Message);

            _mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureIfUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto { Message = "Test message" };
            var command = new CreateNotificationCommand(createNotificationDto);
            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
              .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Notification);
            Assert.Equal("User not found", result.FailureResponse.Message);

            _mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
