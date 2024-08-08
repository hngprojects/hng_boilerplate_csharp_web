using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Domain.Common;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Commands;

public record CreateOrganizationInviteCommand(CreateOrganizationInviteDto InviteDto) : IRequest<Result<OrganizationInviteDto>>;

