using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Queries
{
    public class GetAllNotificationsQuery : IRequest<GetNotificationsResponseDto>
    {
    }
}
