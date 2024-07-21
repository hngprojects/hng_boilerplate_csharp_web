using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository
{
    public class NewsLetterSubscriptionRepository : GenericRepository<NewsLetterSubscriber>, INewsLetterSubscriptionRepository
    {
        private readonly MyDBContext _context;

        public NewsLetterSubscriptionRepository(MyDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddSubscriberAsync(NewsLetterSubscriber subscriber)
        {
            await AddAsync(subscriber);
        }

        public async Task<NewsLetterSubscriber?> GetSubscriberAsync(string email)
        {
            return await _context.NewsLetterSubscribers.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
