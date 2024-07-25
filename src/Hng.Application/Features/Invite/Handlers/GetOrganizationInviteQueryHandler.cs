using AutoMapper;
using Hng.Application.Features.Invite.Dtos;
using Hng.Application.Features.Invite.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Invite.Handlers;

public class GetOrganizationInviteQueryHandler(IRepository<OrganizationInvite> organizationInviteRepository, IMapper mapper)
    : IRequestHandler<GetOrganizationInviteQuery, OrganizationInviteDto>
{
    public async Task<OrganizationInviteDto> Handle(GetOrganizationInviteQuery request, CancellationToken cancellationToken)
    {
        var organizationInvite = await organizationInviteRepository.GetBySpec(
            o => o.Invite_link == request.Token);

        return organizationInvite == null ? null : mapper.Map<OrganizationInviteDto>(organizationInvite);
    }
}