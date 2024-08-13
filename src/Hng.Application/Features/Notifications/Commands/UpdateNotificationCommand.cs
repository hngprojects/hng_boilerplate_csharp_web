using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class UpdateNotificationCommand : IRequest<NotificationDto>
    {
        public Guid NotificationId { get; set; }
        public bool IsRead { get; set; }

        public UpdateNotificationCommand(Guid notificationId, bool isRead)
        {
            NotificationId = notificationId;
            IsRead = isRead;
        }
    }
}
