
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Commands
{
    public class AddSubscriberCommand : IRequest<NewsLetterSubscriptionDto>
    {
        public AddSubscriberCommand(NewsLetterSubscriptionDto newsLetterSubscriptionDto)
        {
            NewsLetterSubscriptionBody = newsLetterSubscriptionDto;
        }

        public NewsLetterSubscriptionDto NewsLetterSubscriptionBody { get; }
    }
}