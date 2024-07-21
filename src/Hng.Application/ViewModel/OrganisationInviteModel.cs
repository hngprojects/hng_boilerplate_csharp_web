using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.ViewModel
{
	public class OrganisationInviteRequestModel
	{
		public List<string> emails { get;set;}
		public string org_id { get;set;}

	}

	public class OrganisationInviteResponseModel
	{
		public string message { get;set;}
		public  List<InvitationResponse> invitations { get;set;}
	}

    public class InvitationResponse
	{
		public string email { get;set;}
		public string org_id { get;set;}
		public DateTime expires_at { get;set;}

	}

}
