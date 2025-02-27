using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Commands
{
    public class DeleteSubscriberCommand : IRequest<SuccessResponseDto<bool>>
    {
        public NewsLetterSubscriptionDto SubscriptionDto { get; }

        public DeleteSubscriberCommand(NewsLetterSubscriptionDto subscriptionDto)
        {
            SubscriptionDto = subscriptionDto;
        }
    }
}