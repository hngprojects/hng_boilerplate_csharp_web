using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.NewsLetterSubscription.Handlers
{
    public class DeleteSubscriberHandler : IRequestHandler<DeleteSubscriberCommand, BaseResponseDto<bool>>
    {
        private readonly IRepository<NewsLetterSubscriber> _repository;

        public DeleteSubscriberHandler(IRepository<NewsLetterSubscriber> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponseDto<bool>> Handle(DeleteSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _repository.GetAsync(request.Id);
            if (subscriber is null)
            {
                return new BaseResponseDto<bool>
                {
                    StatusCode = 404,
                    Message = "Subscriber not found."
                };
            }

            subscriber.IsDeleted = true;
            await _repository.UpdateAsync(subscriber);
            await _repository.SaveChanges();

            return new BaseResponseDto<bool>
            {
                StatusCode = 200,
                Message = "Subscriber deleted successfully."
            };
        }
    }
}