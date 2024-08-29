using System.Net;
using System.Text.Json.Serialization;

namespace Hng.Web.ModelStateError;

public record ModelStateErrorResponse
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; init; } = (int)HttpStatusCode.BadRequest;

    [JsonPropertyName("message")]
    public List<ModelError> Errors { get; init; } = [];

    [JsonPropertyName("data")]

    public object Data { get; } = new { };
}

