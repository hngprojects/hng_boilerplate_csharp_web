using AutoMapper;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

public class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsQuery, GetNotificationsResponseDto>
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public GetAllNotificationsHandler(IRepository<Notification> notificationRepository, IAuthenticationService authenticationService, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    public async Task<GetNotificationsResponseDto> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = await _authenticationService.GetCurrentUserAsync();
        var notifications = await _notificationRepository.GetAllBySpec(n => n.UserId == userId);

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
