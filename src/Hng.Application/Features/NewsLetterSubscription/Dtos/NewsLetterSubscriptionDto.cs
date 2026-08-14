

using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.NewsLetterSubscription.Dtos
{
    public class NewsLetterSubscriptionDto
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}