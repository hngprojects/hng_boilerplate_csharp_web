using Hng.Application.Features.Invite.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Invite.Commands
{
	public class CreateOrganizationInviteCommand : IRequest<OrganizationInviteDto>
	{
		public CreateOrganizationInviteCommand(CreateOrganizationInviteDto createOrganizationInviteDto)
		{
			OrganizationInviteBody = createOrganizationInviteDto;
		}

		public CreateOrganizationInviteDto OrganizationInviteBody { get; }

	}
}
