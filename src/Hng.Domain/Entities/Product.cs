namespace Hng.Domain.Entities;
public class Product : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Status { get; set; } = "In Stock";
    public string ImageUrl { get; set; }
    public string Size { get; set; }
    public int Quantity { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}