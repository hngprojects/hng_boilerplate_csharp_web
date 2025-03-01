using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Commands
{
    public class DeleteSubscriberCommand : IRequest<BaseResponseDto<bool>>
    {
        public Guid Id { get; }

        public DeleteSubscriberCommand(Guid id)
        {
            Id = id;
        }
    }
}