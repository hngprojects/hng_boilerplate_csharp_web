
using System.Text.Json.Serialization;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record GetUniqueOrganizationInviteLinkDto
{
    public string OrganizationId { get; init; }

    [JsonIgnore]
    public Guid UserId { get; set; }

}
