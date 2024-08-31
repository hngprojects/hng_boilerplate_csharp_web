using System.Text.Json.Serialization;

namespace Hng.Application.Features.Squeeze.Dtos;

public class CreateSqueezeRequestDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}