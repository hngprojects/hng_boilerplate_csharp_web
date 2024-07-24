using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities;

public class User : EntityBase
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Required]
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Profile Profile { get; set; }

    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
