using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Dto
{
    public class UpdateRoleRequestDto
    {
        [JsonPropertyName("organizationid")]
        public Guid OrganizationId { get; set; }
        [JsonPropertyName("roleid")]
        public Guid RoleId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        public List<PermissionUpdateModel> Permissions { get; set; }
    }
}
