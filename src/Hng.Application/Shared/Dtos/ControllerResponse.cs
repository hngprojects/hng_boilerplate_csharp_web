using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos;

//useful for annotating methods for swagger
public class ControllerResponse<T>
{

    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }
}

