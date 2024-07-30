namespace Hng.Domain.Entities;

public class Blog : EntityBase
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Content { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool IsPublished { get; set; }
    public User Author { get; set; }
    public BlogCategory Category { get; set; }
}