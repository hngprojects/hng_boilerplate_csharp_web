using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class CreateNotificationSettingsCommand : IRequest<NotificationSettingsDto>
    {
        public CreateNotificationSettingsCommand(CreateNotificationSettingsDto notificationSettingsDto)
        {
            NotificationBody = notificationSettingsDto;
        }
        public CreateNotificationSettingsDto NotificationBody { get; }
    }
}
