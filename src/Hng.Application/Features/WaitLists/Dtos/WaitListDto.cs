using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.WaitLists.Dtos
{
    public class WaitListDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
