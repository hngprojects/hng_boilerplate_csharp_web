using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities;

public class Comment : EntityBase
{
    [Required]
    public string Content { get; set; }
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }

    public DateTime CreatedAt { get; set; }
}