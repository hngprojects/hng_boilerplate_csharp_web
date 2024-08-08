using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Dto
{
    public class DeleteRoleResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
