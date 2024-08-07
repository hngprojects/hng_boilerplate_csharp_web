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
    public class CreateNotificationSettingsHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<NotificationSettings>> _mockNotificationRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly CreateNotificationSettingsHandler _handler;

        public CreateNotificationSettingsHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<NotificationSettings>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new CreateNotificationSettingsHandler(
                _mockNotificationRepository.Object,
                _mockUserRepository.Object,
                _mockAuthenticationService.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateNewNotificationSettingsForNewUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationSettingsDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationSettingsCommand(createNotificationDto);

            var user = new User { Id = userId };
            var newNotification = new NotificationSettings
            {
                UserId = userId,
                EmailNotifications = createNotificationDto.EmailNotifications,
                MobilePushNotifications = createNotificationDto.MobilePushNotifications,
                ActivityWorkspaceEmail = createNotificationDto.ActivityWorkspaceEmail
            };

            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
                .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync((NotificationSettings)null);
            _mockNotificationRepository.Setup(r => r.AddAsync(It.IsAny<NotificationSettings>()))
                .ReturnsAsync(newNotification);
            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(createNotificationDto.EmailNotifications, result.EmailNotifications);
            Assert.Equal(createNotificationDto.MobilePushNotifications, result.MobilePushNotifications);
            Assert.Equal(createNotificationDto.ActivityWorkspaceEmail, result.ActivityWorkspaceEmail);

            _mockNotificationRepository.Verify(r => r.AddAsync(It.Is<NotificationSettings>(n => n.UserId == userId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateExistingNotificationSettings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationSettingsDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationSettingsCommand(createNotificationDto);

            var user = new User { Id = userId };
            var existingNotification = new NotificationSettings
            {
                UserId = userId,
                EmailNotifications = false,
                MobilePushNotifications = true,
                ActivityWorkspaceEmail = false
            };

            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
                .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync(existingNotification);
            _mockNotificationRepository.Setup(r => r.UpdateAsync(It.IsAny<NotificationSettings>()))
                .Returns(Task.CompletedTask);
            _mockNotificationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(createNotificationDto.EmailNotifications, result.EmailNotifications);
            Assert.Equal(createNotificationDto.MobilePushNotifications, result.MobilePushNotifications);
            Assert.Equal(createNotificationDto.ActivityWorkspaceEmail, result.ActivityWorkspaceEmail);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.Is<NotificationSettings>(n => n.UserId == userId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationSettingsDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationSettingsCommand(createNotificationDto);

            _mockAuthenticationService.Setup(auth => auth.GetCurrentUserAsync())
                .ReturnsAsync(userId);
            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<NotificationSettings>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<NotificationSettings>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
