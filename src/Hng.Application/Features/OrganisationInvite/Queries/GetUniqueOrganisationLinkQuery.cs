using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Queries;

public record GetUniqueOrganizationLinkQuery(GetUniqueOrganizationInviteLinkDto Details) : IRequest<StatusCodeResponse>;
