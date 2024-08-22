
using System.Text.Json.Serialization;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record CreateUniqueOrganizationInviteLinkDto
{
    [JsonPropertyName("org_id")]
    public string OrganizationId { get; init; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
