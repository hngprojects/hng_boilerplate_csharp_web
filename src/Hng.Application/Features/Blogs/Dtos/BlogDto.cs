using System.Text.Json.Serialization;
using Hng.Domain.Entities;
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
    [JsonPropertyName("published-date")]
    public DateTime PublishedDate { get; set; }
    [JsonPropertyName("author")]
    public User Author { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("category")]
    public BlogCategory Category { get; set; }
}