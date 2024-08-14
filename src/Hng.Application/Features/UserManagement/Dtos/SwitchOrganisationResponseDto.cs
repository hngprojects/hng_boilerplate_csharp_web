using System.Text.Json.Serialization;
using Hng.Application.Features.Organisations.Dtos;

namespace Hng.Application.Features.UserManagement.Dtos;

public class SwitchOrganisationResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("status_code")]
    public int StatusCode { get; init; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OrganizationDto OrganisationDto { get; init; }
}