using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record CreateOrganizationInviteDto
{
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("org_id")]
    public string OrganizationId { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
