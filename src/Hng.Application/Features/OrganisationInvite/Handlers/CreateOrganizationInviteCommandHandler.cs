using AutoMapper;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class CreateOrganizationInviteCommandHandler(IOrganizationInviteService service, IMapper mapper) : IRequestHandler<CreateOrganizationInviteCommand, OrganizationInviteDto>
{
    private readonly IOrganizationInviteService service = service;

    public async Task<OrganizationInviteDto> Handle(CreateOrganizationInviteCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.InviteDto.OrganizationId, out Guid orgId)) return null;

        var organizationInvite = await service.CreateInvite(request.InviteDto.UserId, orgId, request.InviteDto.Email);

        var dto = mapper.Map<OrganizationInviteDto>(organizationInvite);
        return dto;
    }
}
