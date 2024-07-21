using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities;

public class User : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Profile Profile { get; set; }

    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
