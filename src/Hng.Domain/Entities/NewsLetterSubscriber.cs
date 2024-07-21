namespace Hng.Domain.Entities
{
    public class NewsLetterSubscriber : EntityBase
    {
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftOn { get; set; } = null;
    }
}
