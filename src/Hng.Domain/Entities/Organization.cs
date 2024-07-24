using System.Text.Json.Serialization;

namespace Hng.Domain.Entities;

public class Organization : EntityBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Slug { get; set; }

    public string Email { get; set; }

    public string Industry { get; set; }

    public string Type { get; set; }

    public string Country { get; set; }

    public string Address { get; set; }

    public string State { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid OwnerId { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
