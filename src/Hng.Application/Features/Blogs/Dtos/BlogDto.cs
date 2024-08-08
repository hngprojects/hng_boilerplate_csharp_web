using System.Text.Json.Serialization;
using Hng.Application.Features.Comments.Dtos;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Blogs.Dtos;

public class BlogDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
    [JsonPropertyName("published_date")]
    public DateTime PublishedDate { get; set; }
    
    [JsonPropertyName("updated_date")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("author_id")]
    public Guid AuthorId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("category")]
    public BlogCategory Category { get; set; }
    [JsonPropertyName("comments")]
    public ICollection<CommentDto> Comments { get; set; }
}