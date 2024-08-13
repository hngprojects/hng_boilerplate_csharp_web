
using Hng.Application.Features.Organisations.Dtos;

namespace Hng.Application.Features.UserManagement.Dtos;

public class SwitchOrganisationResponseDto
{
    public string Message { get; set; }
    public OrganizationDto OrganisationDto { get; set; }
}