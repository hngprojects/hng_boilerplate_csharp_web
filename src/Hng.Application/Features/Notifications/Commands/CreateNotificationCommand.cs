using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationDto>
    {
        public CreateNotificationCommand(CreateNotificationDto notificationDto, string loggedInUserId)
        {
            NotificationBody = notificationDto;
            LoggedInUserId = loggedInUserId;
        }
        public CreateNotificationDto NotificationBody { get; }
        public string LoggedInUserId { get; }
    }
}
