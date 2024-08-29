using System.Text.Json.Serialization;

namespace Hng.Application.Features.Jobs.Dtos;

public class GetJobRequestDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JobDto Data { get; set; }
}