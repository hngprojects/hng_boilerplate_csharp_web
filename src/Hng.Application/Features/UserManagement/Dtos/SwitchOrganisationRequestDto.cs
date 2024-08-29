using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.UserManagement.Dtos;

public class SwitchOrganisationRequestDto
{
    [Required]
    public bool IsActive { get; set; }
}