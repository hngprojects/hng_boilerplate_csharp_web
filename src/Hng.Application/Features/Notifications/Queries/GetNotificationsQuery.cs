using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<GetNotificationsResponseDto>
    {
        public bool? IsRead { get; set; }

        public GetNotificationsQuery(bool? isRead)
        {
            IsRead = isRead;
        }
    }
}
