using System.Text.Json.Serialization;
using Hng.Application.Shared.Validators;

namespace Hng.Application.Features.OrganisationInvite.Dtos;

public record AcceptInviteDto
{
    [JsonPropertyName("invite_token_guid")]
    [ValidGuid]
    public string Token { get; init; }

}
