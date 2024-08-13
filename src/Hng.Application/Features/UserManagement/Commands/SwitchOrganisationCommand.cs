using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Commands;

public class SwitchOrganisationCommand : IRequest<SwitchOrganisationResponseDto>
{
    public Guid UserId { get; set; }
    public Guid OrganisationId { get; set; }
    public bool IsActive { get; set; }
}