using Hng.Application.Dto;

namespace Hng.Application.Interfaces
{
    public interface INewsLetterSubscriptionService
    {
        Task<NewsLetterSubscriptionDto?> AddToNewsLetter(NewsLetterSubscriptionDto subscriber);
    }
}
