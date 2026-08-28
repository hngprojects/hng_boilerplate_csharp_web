using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Comments.Dtos;

public class UpdateCommentDto
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}