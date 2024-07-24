using Hng.Application.Features.Organisations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Invite.Commands
{
	public class CreateInviteCommand : IRequest<OrganizationDto>
	{
		public CreateinviteCommand(CreateInviteDto createinviteDto)
		{
			OrganizationBody = createOrganizationDto;
		}

		public CreateOrganizationDto OrganizationBody { get; }

	}
}
