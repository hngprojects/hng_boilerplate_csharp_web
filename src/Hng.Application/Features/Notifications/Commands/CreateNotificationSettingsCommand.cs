using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationSettingsCommand : IRequest<NotificationSettingsDto>
    {
        public CreateNotificationSettingsCommand(CreateNotificationSettingsDto notificationSettingsDto, string loggedInUserId)
        {
            NotificationBody = notificationSettingsDto;
            LoggedInUserId = loggedInUserId;
        }
        public CreateNotificationSettingsDto NotificationBody { get; }
        public string LoggedInUserId { get; }
    }
}
