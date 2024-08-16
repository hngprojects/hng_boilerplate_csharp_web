using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos;

//useful for annotating methods for swagger
public class ControllerErrorResponse
{

    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public List<Error> Errors { get; set; }

    [JsonPropertyName("data")]
    public EmptyDataResponse Data { get; set; }
}

public record Error
{
    [JsonPropertyName("field")]
    public string Field { get; init; }

    [JsonPropertyName("error_message")]
    public string ErrorMessage { get; init; }
}
