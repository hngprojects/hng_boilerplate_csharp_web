
namespace Hng.Domain.Entities;
    public class Product: EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
