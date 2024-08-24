using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Dto
{
    public class UpdateRoleResponseDto
    {
        public int StatusCode { get; set; }
        [JsonPropertyName("role_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }

}
