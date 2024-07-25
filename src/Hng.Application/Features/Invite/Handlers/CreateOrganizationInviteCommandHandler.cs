using AutoMapper;
using Hng.Application.Features.Invite.Commands;
using Hng.Application.Features.Invite.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Invite.Handlers;

public class CreateOrganizationInviteCommandHandler : IRequestHandler<CreateOrganizationInviteCommand, OrganizationInviteDto>
{
    private IRepository<OrganizationInvite> _organizationInviteRepository;
    private IMapper _mapper;


    public CreateOrganizationInviteCommandHandler(IRepository<OrganizationInvite> organizationInviteRepository, IMapper mapper)
    {
        _organizationInviteRepository = organizationInviteRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationDto> Handle(CreateOrganizationInviteCommand request, CancellationToken cancellationToken)
    {
        var organizationInvite = new OrganizationInvite();

        organizationInvite.Organization_id = null;
        organizationInvite.Name = null;
        organizationInvite.Invite_email = null;
        organizationInvite.Status = null;
        organizationInvite.Created_at = null;
        organizationInvite.Expires_at = null;
        organizationInvite.Accepted_at = null;
        organizationInvite.Invite_link = request.OrganizationInviteBody.InvitationLink;
        organizationInvite.CreatedAt = DateTime.UtcNow;
        organizationInvite.UpdatedAt = DateTime.UtcNow;

        await _organizationInviteRepository.AddAsync(organization);
        await _organizationInviteRepository.SaveChanges();
        return _mapper.Map<OrganizationInviteDto>(organizationInvite);
    }
}