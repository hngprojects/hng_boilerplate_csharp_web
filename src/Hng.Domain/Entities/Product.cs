
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities;
public class Product : EntityBase
{
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public new int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Category { get; set; }
    public decimal Price { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
