
namespace Hng.Domain.Entities;
public class Product : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
<<<<<<<<< Temporary merge branch 1

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
=========
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
