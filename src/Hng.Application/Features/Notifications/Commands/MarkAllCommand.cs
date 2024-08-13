using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Commands
{
    public class MarkAllCommand : IRequest<List<NotificationDto>>
    {
        public bool IsRead { get; set; }

        public MarkAllCommand(bool isRead)
        {
            IsRead = isRead;
        }
    }
}
