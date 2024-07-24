using Hng.Application.Features.OrganisationInvite.Dtos;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Commands;

public record CreateOrganizationInviteCommand(CreateOrganizationInviteDto InviteDto): IRequest<OrganizationInviteDto>;

