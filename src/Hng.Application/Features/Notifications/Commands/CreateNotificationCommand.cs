using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationResult>
    {
        public CreateNotificationCommand(CreateNotificationDto createNotificationDto, string loggedInUserId)
        {
            Notification = createNotificationDto;
            LoggedInUserId = loggedInUserId;
        }
        public CreateNotificationDto Notification { get; }
        public string LoggedInUserId { get; }
    }
}
