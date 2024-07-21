using Hng.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
	public interface IOrganisationService
	{
		Task<OrganisationInviteResponseModel> SendInvites(OrganisationInviteRequestModel request);
	}
}
