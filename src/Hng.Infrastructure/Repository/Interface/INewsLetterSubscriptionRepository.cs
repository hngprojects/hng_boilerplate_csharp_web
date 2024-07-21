using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface INewsLetterSubscriptionRepository
    {
        Task AddSubscriberAsync(NewsLetterSubscriber subscriber);

        Task<NewsLetterSubscriber> GetSubscriberAsync(string email);
    }
}
