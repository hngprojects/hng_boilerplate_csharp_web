using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Commands;

public record AcceptInviteCommand(AcceptInviteDto InviteCode) : IRequest<StatusCodeResponse>;
