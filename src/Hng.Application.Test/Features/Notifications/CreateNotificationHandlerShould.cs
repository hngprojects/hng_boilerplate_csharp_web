﻿using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Handlers;
using Hng.Application.Features.Notifications.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;
using System.Linq.Expressions;
using Hng.Application.Features.Notifications.Dtos;

namespace Hng.Application.Test.Features.Notifications
{
    public class CreateNotificationHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly CreateNotificationHandler _handler;

        public CreateNotificationHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _handler = new CreateNotificationHandler(_mockNotificationRepository.Object, _mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateNewNotificationSettingsForNewUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationCommand(createNotificationDto, userId.ToString());

            var user = new User { Id = userId };
            var newNotification = new Notification
            {
                UserId = userId,
                EmailNotifications = createNotificationDto.EmailNotifications,
                MobilePushNotifications = createNotificationDto.MobilePushNotifications,
                ActivityWorkspaceEmail = createNotificationDto.ActivityWorkspaceEmail
            };

            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync((Notification)null);

            _mockNotificationRepository.Setup(r => r.AddAsync(It.IsAny<Notification>()))
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

            _mockNotificationRepository.Verify(r => r.AddAsync(It.Is<Notification>(n => n.UserId == userId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateExistingNotificationSettings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationCommand(createNotificationDto, userId.ToString());

            var user = new User { Id = userId };
            var existingNotification = new Notification
            {
                UserId = userId,
                EmailNotifications = false,
                MobilePushNotifications = true,
                ActivityWorkspaceEmail = false
            };

            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

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
            Assert.Equal(userId, result.UserId);
            Assert.Equal(createNotificationDto.EmailNotifications, result.EmailNotifications);
            Assert.Equal(createNotificationDto.MobilePushNotifications, result.MobilePushNotifications);
            Assert.Equal(createNotificationDto.ActivityWorkspaceEmail, result.ActivityWorkspaceEmail);

            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.Is<Notification>(n => n.UserId == userId)), Times.Once);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createNotificationDto = new CreateNotificationDto
            {
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            var command = new CreateNotificationCommand(createNotificationDto, userId.ToString());

            _mockUserRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
