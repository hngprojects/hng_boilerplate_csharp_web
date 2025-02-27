using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Handlers
{
    public class DeleteSubscriberHandler : IRequestHandler<DeleteSubscriberCommand, SuccessResponseDto<bool>>
    {
        private readonly IRepository<NewsLetterSubscriber> _repository;

        public DeleteSubscriberHandler(IRepository<NewsLetterSubscriber> repository)
        {
            _repository = repository;
        }

        public async Task<SuccessResponseDto<bool>> Handle(DeleteSubscriberCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SubscriptionDto?.Email))
            {
                return new SuccessResponseDto<bool>
                {
                    StatusCode = 400,
                    Message = "Invalid email address.",
                    Data = false
                };
            }

            var subscriber = await _repository.GetBySpec(
                s => s.Email == request.SubscriptionDto.Email
            );
            if (subscriber is null)
            {
                return new SuccessResponseDto<bool>
                {
                    StatusCode = 404,
                    Message = "Subscriber not found.",
                    Data = false
                };
            }

            await _repository.DeleteAsync(subscriber);
            await _repository.SaveChanges();
            return new SuccessResponseDto<bool>
            {
                StatusCode = 200,
                Message = "Subscriber deleted successfully.",
                Data = true
            };
        }
    }
}