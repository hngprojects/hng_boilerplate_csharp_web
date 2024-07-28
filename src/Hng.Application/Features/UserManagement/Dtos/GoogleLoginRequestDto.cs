using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class GoogleLoginRequestDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
