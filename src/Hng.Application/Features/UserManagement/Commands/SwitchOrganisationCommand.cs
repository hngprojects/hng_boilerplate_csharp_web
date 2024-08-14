using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands;

public class SwitchOrganisationCommand : IRequest<SwitchOrganisationResponseDto>
{
    public Guid OrganisationId { get; set; }
    public bool IsActive { get; set; }
}