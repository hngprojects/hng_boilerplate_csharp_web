using System.Text.Json.Serialization;

namespace Hng.Application.Features.Squeeze.Dtos;

public class CreateSqueezeResponseDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SqueezeDto Data { get; set; }
}