using System.Text.Json.Serialization;

namespace Hng.Web.ModelStateError;

public record ModelError
{
    [JsonPropertyName("field")]
    public string Field { get; init; }


    [JsonPropertyName("message")]
    public string Message { get; init; }
}

