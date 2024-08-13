using Hng.Application.Features.OrganisationInvite.Dtos;
using MediatR;
using Hng.Application.Shared.Dtos;

namespace Hng.Application.Features.OrganisationInvite.Commands;

public record CreateOrganizationInviteCommand(CreateOrganizationInviteDto InviteDto) : IRequest<StatusCodeResponse<OrganizationInviteDto>>;

