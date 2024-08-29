using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Queries
{
    public class GetSubscriptionsQuery : IRequest<PagedListDto<SubscriptionDto>>
    {
        public GetSubscriptionsQuery(GetSubscriptionsQueryParameters parameters)
        {
            subscriptionsQueryParameters = parameters;
        }

        public GetSubscriptionsQueryParameters subscriptionsQueryParameters { get; set; }
    }
}
