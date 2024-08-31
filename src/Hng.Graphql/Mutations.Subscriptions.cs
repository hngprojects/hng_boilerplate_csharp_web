using Hng.Application.Features.Subscriptions.Commands;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Hng.Graphql
{
    public partial class Mutations
    {
        //[Authorize]
        //public async Task<SubscribeFreePlanResponse> SubscribeFreePlan(SubscribeFreePlan command, [FromServices] IMediator mediator)
        //{
        //    //var command = new SubscribeFreePlan();
        //   // return await mediator.Send(command);


        //   // return await mediator.Send(command);
        //}

        [Authorize]
        public async Task<SubscriptionDto> ActivateSubscription(Guid subscriptionId, [FromServices] IMediator mediator)
        {
            var command = new ActivateSubscriptionCommand(subscriptionId);
            return await mediator.Send(command);
        }
    }
}