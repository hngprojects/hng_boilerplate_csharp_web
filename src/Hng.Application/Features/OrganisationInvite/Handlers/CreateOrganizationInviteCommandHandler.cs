using AutoMapper;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Domain.Common;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class CreateOrganizationInviteCommandHandler(IOrganizationInviteService service, IMapper mapper) : IRequestHandler<CreateOrganizationInviteCommand, Result<OrganizationInviteDto>>
{
    private readonly IOrganizationInviteService service = service;

    public async Task<Result<OrganizationInviteDto>> Handle(CreateOrganizationInviteCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.InviteDto.OrganizationId, out Guid orgId)) return Result<OrganizationInviteDto>.Failure(new Error("An Invalid organization id was passed"));

        var inviteResult = await service.CreateInvite(request.InviteDto.UserId, orgId, request.InviteDto.Email);

        if (!inviteResult.IsSuccess)
        {
            return Result<OrganizationInviteDto>.Failure(inviteResult.Error);
        }
        var dto = mapper.Map<OrganizationInviteDto>(inviteResult.Value);
        return Result<OrganizationInviteDto>.Success(dto);
    }
}
