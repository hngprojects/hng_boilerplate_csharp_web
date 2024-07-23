namespace Hng.Domain.Entities;

public class User : EntityBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public Profile Profile { get; set; }

    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
