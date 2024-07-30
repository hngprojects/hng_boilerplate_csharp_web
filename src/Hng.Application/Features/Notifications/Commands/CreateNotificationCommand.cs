using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationDto>
    {
        public CreateNotificationCommand(CreateNotificationDto notificationDto)
        {
            NotificationBody = notificationDto;
        }
        public CreateNotificationDto NotificationBody { get; }
    }
}
