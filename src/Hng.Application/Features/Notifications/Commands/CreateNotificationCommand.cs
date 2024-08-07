using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationResult>
    {
        public CreateNotificationCommand(CreateNotificationDto createNotificationDto)
        {
            Notification = createNotificationDto;
        }
        public CreateNotificationDto Notification { get; }
    }
}
