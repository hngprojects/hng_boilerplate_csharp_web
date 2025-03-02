using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Commands
{
    public class DeleteSubscriberByEmailCommand : IRequest<BaseResponseDto<bool>>
    {
        public NewsLetterSubscriptionDto Dto { get; }

        public DeleteSubscriberByEmailCommand(NewsLetterSubscriptionDto dto)
        {
            Dto = dto;
        }
    }
}