

using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.NewsLetterSubscription.Dtos
{
    public class NewsLetterSubscriptionDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}