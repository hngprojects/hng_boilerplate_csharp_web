using AutoMapper;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Hng.Infrastructure.Services.Interfaces;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, GetNotificationsResponseDto>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public GetNotificationsHandler(
            IRepository<Notification> notificationRepository,
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<GetNotificationsResponseDto> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var notifications = await _notificationRepository.GetAllBySpec(n => n.UserId == userId);

            if (request.IsRead.HasValue)
            {
                notifications = notifications.Where(n => n.IsRead == request.IsRead.Value).ToList();
            }

            var totalNotificationCount = notifications.Count();
            var totalUnreadNotificationCount = notifications.Count(n => !n.IsRead);

            var notificationDtos = _mapper.Map<List<NotificationDto>>(notifications);

            return new GetNotificationsResponseDto
            {
                TotalNotificationCount = totalNotificationCount,
                TotalUnreadNotificationCount = totalUnreadNotificationCount,
                Notifications = notificationDtos
            };
        }
    }
}
