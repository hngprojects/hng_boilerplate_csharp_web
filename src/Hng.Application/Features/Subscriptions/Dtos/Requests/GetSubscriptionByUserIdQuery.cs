using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public class GetSubscriptionByUserIdQuery : IRequest<SubscriptionDto>
    {
        public Guid UserId { get; set; }

        public GetSubscriptionByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}