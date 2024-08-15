using System.Text.Json.Serialization;

namespace Hng.Application.Features.Organisations.Dtos;

public class CreateOrganisationResponseDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OrganizationDto Data { get; set; }
}