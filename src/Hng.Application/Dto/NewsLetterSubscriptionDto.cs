using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Dto
{
    public class NewsLetterSubscriptionDto
    {
        [EmailAddress]
        public string email { get; set; }
    }
}
