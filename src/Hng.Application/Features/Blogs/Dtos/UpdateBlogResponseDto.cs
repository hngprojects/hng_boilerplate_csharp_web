using System.Text.Json.Serialization;
using Hng.Application.Features.Comments.Dtos;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Blogs.Dtos;

public class UpdateBlogResponseDto
{
    [JsonPropertyName("blog_id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("tags")]
    public List<List<string>> Tags { get; set; } = new();

    [JsonPropertyName("image_urls")]
    public List<List<string>> ImageUrls { get; set; } = new();

    [JsonPropertyName("author")]
    public string Author { get; set; }
    [JsonPropertyName("created_date")]
    public DateTime PublishedDate { get; set; }
    
}