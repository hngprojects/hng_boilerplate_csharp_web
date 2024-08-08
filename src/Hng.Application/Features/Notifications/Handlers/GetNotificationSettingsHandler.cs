using AutoMapper;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class GetNotificationSettingsHandler : IRequestHandler<GetNotificationSettingsQuery, NotificationDto>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationSettingsHandler(IRepository<Notification> notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(GetNotificationSettingsQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetBySpec(n => n.UserId == request.UserId);
            return notification != null ? _mapper.Map<NotificationDto>(notification) : null;
        }
    }
}
