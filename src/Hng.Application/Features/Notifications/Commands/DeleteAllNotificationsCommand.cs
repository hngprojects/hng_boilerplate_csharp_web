using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class DeleteAllNotificationsCommand : IRequest<bool>
    {
        public DeleteAllNotificationsCommand()
        {
        }
    }
}
