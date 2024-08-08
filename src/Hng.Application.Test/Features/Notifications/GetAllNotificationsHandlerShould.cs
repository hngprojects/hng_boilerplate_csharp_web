using AutoMapper;
using Hng.Application.Features.Notifications.Mappers;
using Hng.Application.Features.Notifications.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Notifications
{
    public class GetAllNotificationsHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Notification>> _mockNotificationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly GetAllNotificationsHandler _handler;

        public GetAllNotificationsHandlerShould()
        {
            var mappingProfile = new NotificationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockNotificationRepository = new Mock<IRepository<Notification>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _handler = new GetAllNotificationsHandler(_mockNotificationRepository.Object, _mockAuthenticationService.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotificationsWhenFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetAllNotificationsQuery();

            var notifications = new List<Notification>
            {
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "New notification", CreatedAt = DateTime.UtcNow },
                new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "Old notification", CreatedAt = DateTime.UtcNow }
            };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(notifications);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notifications.Count, result.TotalNotificationCount);
            Assert.Equal(notifications.Count(n => !n.IsRead), result.TotalUnreadNotificationCount);
            Assert.Equal(notifications.Count, result.Notifications.Count);

            _mockNotificationRepository.Verify(r => r.GetAllBySpec(It.Is<Expression<Func<Notification, bool>>>(expr => expr.Compile()(notifications[0]))), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyWhenNoNotificationsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetAllNotificationsQuery();

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockNotificationRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalNotificationCount);
            Assert.Equal(0, result.TotalUnreadNotificationCount);
            Assert.Empty(result.Notifications);

            _mockNotificationRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Notification, bool>>>()), Times.Once);
        }
    }
}
