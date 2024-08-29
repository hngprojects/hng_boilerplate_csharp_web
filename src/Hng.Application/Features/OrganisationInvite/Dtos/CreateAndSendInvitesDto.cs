using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Hng.Application.Shared.Dtos.Validators;
using Hng.Application.Shared.Validators;
using Hng.Domain.Entities;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record CreateAndSendInvitesDto
{

    [JsonPropertyName("emails")]
    [EmailCollection]
    [MinLength(1, ErrorMessage = "The email list cannot be empty")]
    public IEnumerable<string> Emails { get; init; }

    [JsonPropertyName("org_id")]
    [ValidGuid(AllowEmpty = true)]
    public string OrgId { get; init; }

    [JsonIgnore]
    public Guid InviterId { get; set; }

}
