using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class DeleteNotificationByIdCommand : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public DeleteNotificationByIdCommand(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
