using System.Text.Json.Serialization;

namespace Hng.Application.Features.Blogs.Dtos;

public class CreateBlogResponseDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("data")]
    public BlogDto Data { get; init; }
}