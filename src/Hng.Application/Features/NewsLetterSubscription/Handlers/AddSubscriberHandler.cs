
using Hng.Application.Features.Organisations.Commands;
using Hng.Infrastructure.Repository.Interface;
using Hng.Application.Features.Organisations.Dtos;
using MediatR;
using Hng.Domain.Entities;
using AutoMapper;
using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;

namespace Hng.Application.Features.NewsLetterSubscription.Handlers
{

    public class AddSubscriberHandler : IRequestHandler<AddSubscriberCommand, NewsLetterSubscriptionDto>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<NewsLetterSubscriber> _repository;
        public AddSubscriberHandler(IRepository<NewsLetterSubscriber> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        public async Task<NewsLetterSubscriptionDto> Handle(AddSubscriberCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _repository.GetBySpec(u => u.Email == request.NewsLetterSubscriptionBody.Email);
            if (userExists is not null)
                return null;
            var subscriberModel = _mapper.Map<NewsLetterSubscriber>(request.NewsLetterSubscriptionBody);

            await _repository.AddAsync(subscriberModel);
            await _repository.SaveChanges();
            return request.NewsLetterSubscriptionBody;
        }
    }
}