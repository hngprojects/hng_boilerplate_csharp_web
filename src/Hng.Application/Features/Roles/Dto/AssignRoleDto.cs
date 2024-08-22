using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Dto
{
    public class AssignRoleDto
    {
        [JsonPropertyName("user_id")]
        public Guid Id { get; set; }
    }
}
