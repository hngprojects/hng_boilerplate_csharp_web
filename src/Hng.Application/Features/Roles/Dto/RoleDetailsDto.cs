using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Dto
{
    public class RoleDetailsDto
    {
        public int StatusCode { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PermissionDto> Permissions { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
    }

    public class PermissionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
