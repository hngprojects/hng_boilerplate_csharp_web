using Hng.Application.Features.Invite.Dtos;
using MediatR;

namespace Hng.Application.Features.Invite.Queries;

public class GetOrganizationInviteQuery(string token) : IRequest<OrganizationInviteDto>
{
    public string Token { get; } = id;
}