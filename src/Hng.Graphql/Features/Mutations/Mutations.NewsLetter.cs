using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        public async Task<NewsLetterSubscriptionDto> RegisterNewsLetterSubscriber(NewsLetterSubscriptionDto subscriber, [FromServices] IMediator mediator)
        {
            return await mediator.Send(new AddSubscriberCommand(subscriber));
        }
    }
}
