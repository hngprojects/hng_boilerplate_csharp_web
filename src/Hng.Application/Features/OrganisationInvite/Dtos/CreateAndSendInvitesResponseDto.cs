using System.Text.Json.Serialization;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record CreateAndSendInvitesResponseDto
{
    [JsonPropertyName("invitations")]
    public IEnumerable<InviteDto> Invitations { get; set; }
}

public record InviteDto
{
    [JsonPropertyName("email")]
    public string Email { get; init; }

    [JsonPropertyName("invite_link")]
    public string InviteLink { get; set; } = string.Empty;

    [JsonPropertyName("error")]

    public string Error { get; set; } = string.Empty;
}