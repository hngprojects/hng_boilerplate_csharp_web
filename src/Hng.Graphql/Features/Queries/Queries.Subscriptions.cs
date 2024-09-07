using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Subscriptions.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<SubscriptionDto> GetSubscriptionByOrganizationId(Guid organizationId, [FromServices] IMediator mediator)
        {
            var response = new GetSubscriptionByOrganizationIdQuery(organizationId);
            return await mediator.Send(response);
        }

        [Authorize]
        public async Task<SubscriptionDto> GetSubscriptionByUserId(Guid userId, [FromServices] IMediator mediator)
        {
            var response = new GetSubscriptionByUserIdQuery(userId);
            return await mediator.Send(response);
        }

        [Authorize]
        public async Task<PagedListDto<SubscriptionDto>> GetSubscriptions(GetSubscriptionsQueryParameters parameters, [FromServices] IMediator mediator)
        {
            var subscriptions = new GetSubscriptionsQuery(parameters);
            return await mediator.Send(subscriptions);
        }
    }
}