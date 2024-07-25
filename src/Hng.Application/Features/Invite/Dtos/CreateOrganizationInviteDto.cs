using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Invite.Dtos
{
    public class CreateOrganizationInviteDto
    {
        [JsonPropertyName("invitationLink")]
        public string InvitationLink { get; set; }
    }
}
