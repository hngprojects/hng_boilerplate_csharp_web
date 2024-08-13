using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationDto>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;

        public UpdateNotificationCommandHandler(IRepository<Notification> notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetBySpec(n => n.Id == request.NotificationId);

            if (notification != null)
            {
                notification.IsRead = request.IsRead;
                notification.UpdatedAt = DateTime.UtcNow;
                await _notificationRepository.UpdateAsync(notification);
                await _notificationRepository.SaveChanges();
                await _notificationRepository.SaveChanges();
                return _mapper.Map<NotificationDto>(notification);
            }
            return null;
        }
    }
}
