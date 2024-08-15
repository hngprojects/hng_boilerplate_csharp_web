using System.Net;
using System.Text.Json.Serialization;

namespace Hng.Web.ModelStateError;

public record ModelStateErrorResponse
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; init; } = (int)HttpStatusCode.BadRequest;

    [JsonPropertyName("message")]
    public string Message { get; init; } = "Invalid request parameter(s) passed";

    [JsonPropertyName("errors")]
    public List<ModelError> Errors { get; init; } = [];

    [JsonPropertyName("data")]

    public object Data { get; } = new { };
}

