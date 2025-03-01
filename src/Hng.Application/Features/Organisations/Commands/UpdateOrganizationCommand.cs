using Hng.Application.Features.Organisations.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Commands;

public class UpdateOrganizationCommand : IRequest<UpdateOrganizationResponseDto>
{
    public UpdateOrganizationCommand(Guid orgId, UpdateOrganizationDto updateOrganizationDto)
    {
        OrgId = orgId;
        OrganizationBody = updateOrganizationDto;
    }

    public Guid OrgId { get; }
    public UpdateOrganizationDto OrganizationBody { get; }
}
