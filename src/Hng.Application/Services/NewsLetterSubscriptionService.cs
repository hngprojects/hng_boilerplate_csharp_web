using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Application.Services
{
    public class NewsLetterSubscriptionService : INewsLetterSubscriptionService
    {
        private readonly INewsLetterSubscriptionRepository _newsLetterSubscriptionRepository;
        private readonly IMapper _mapper;

        public NewsLetterSubscriptionService(INewsLetterSubscriptionRepository newsLetterSubscriptionRepository, IMapper mapper)
        {
            _newsLetterSubscriptionRepository = newsLetterSubscriptionRepository;
            _mapper = mapper;
        }
        public async Task<NewsLetterSubscriptionDto?> AddToNewsLetter(NewsLetterSubscriptionDto subscriber)
        {
            var userExists = await _newsLetterSubscriptionRepository.GetSubscriberAsync(subscriber.email);
            if (userExists is not null)
                return null;
            var subscriberModel = _mapper.Map<NewsLetterSubscriber>(subscriber);
            await _newsLetterSubscriptionRepository.AddSubscriberAsync(subscriberModel);
            return subscriber;
        }
    }
}
