using System.Text.Json.Serialization;
using Hng.Domain.Entities;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Blogs.Dtos;

public class CreateBlogDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("category")]
    public BlogCategory Category { get; set; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
}

