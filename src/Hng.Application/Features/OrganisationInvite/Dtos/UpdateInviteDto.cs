using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Hng.Application.Shared.Validators;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record UpdateInviteDto
{
    [ValidGuid(AllowEmpty = false)]
    [JsonPropertyName("organisation_id")]
    public string OrganizationId { get; init; }

    [JsonPropertyName("status")]
    [EnumDataType(typeof(OrgInviteStatus))]
    public string InviteStatus { get; init; }

    [JsonPropertyName("expires_at")]
    public DateTimeOffset ExpiresAt { get; init; }
}

[JsonConverter(typeof(OrgInviteStatus))]
public enum OrgInviteStatus
{
    [Description("Invitation is pending")]
    Pending,

    [Description("Invitation has been accepted")]
    Accepted,

    [Description("Invitation has expired")]
    Expired,

    [Description("Invitation has been revoked")]
    Revoked
}