namespace Hng.Domain.Models
{
    public class Product: EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}