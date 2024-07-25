using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Invite.Dtos
{
	public class OrganizationInviteDto
	{
        public Guid Id { get; set; }
        [JsonPropertyName("organization_id")]
        public Guid Organization_id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("invite_email")]
        public string Invite_email { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime Created_at { get; set; }
        [JsonPropertyName("expires_at")]
        public DateTime Expires_at { get; set; }
        [JsonPropertyName("accepted_at")]
        public DateTime Accepted_at { get; set; }
        [JsonPropertyName("invite_link")]
        public string Invite_link { get; set; }
    }
}
