using System.Text.Json.Serialization;

namespace Hng.Application.Features.Blogs.Dtos;

public class UpdateBlogResponseDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BlogDto Data { get; set; }
}