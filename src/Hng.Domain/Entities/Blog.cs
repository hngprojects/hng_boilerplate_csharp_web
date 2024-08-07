using System.ComponentModel.DataAnnotations;
using Hng.Domain.Enums;

namespace Hng.Domain.Entities;

public class Blog : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public BlogCategory Category { get; set; }
    public ICollection<Comment> Comments { get; set; }
}