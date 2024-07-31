using System.Text.Json.Serialization;

namespace Hng.Application.Features.Comments;

public class CommentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}