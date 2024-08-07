using AutoMapper;
using Hng.Application.Features.Notifications.Handlers;
using Hng.Application.Features.Notifications.Mappers;
using Hng.Application.Features.Notifications.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Notifications
{
    public class GetNotificationSettingsHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<NotificationSettings>> _mockNotificationRepository;
        private readonly GetNotificationSettingsHandler _handler;

        public GetNotificationSettingsHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<NotificationSettings>>();
            _handler = new GetNotificationSettingsHandler(_mockNotificationRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotificationSettingsWhenFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetNotificationSettingsQuery(userId);

            var existingNotification = new NotificationSettings
            {
                UserId = userId,
                EmailNotifications = true,
                MobilePushNotifications = false,
                ActivityWorkspaceEmail = true
            };

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync(existingNotification);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(existingNotification.EmailNotifications, result.EmailNotifications);
            Assert.Equal(existingNotification.MobilePushNotifications, result.MobilePushNotifications);
            Assert.Equal(existingNotification.ActivityWorkspaceEmail, result.ActivityWorkspaceEmail);

            _mockNotificationRepository.Verify(r => r.GetBySpec(It.Is<Expression<Func<NotificationSettings, bool>>>(expr => expr.Compile()(existingNotification))), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenNotificationSettingsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetNotificationSettingsQuery(userId);

            _mockNotificationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()))
                .ReturnsAsync((NotificationSettings)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockNotificationRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<NotificationSettings, bool>>>()), Times.Once);
        }
    }
}
