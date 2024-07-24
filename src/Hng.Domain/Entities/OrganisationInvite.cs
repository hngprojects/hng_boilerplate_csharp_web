using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
	public class OrganisationInvite
	{
		public Guid Id { get; set; }
		public Guid Organization_id { get; set; }
		public string Name { get; set; }
		public string Invite_email { get; set; }
		public string Status { get; set; }
		public DateTime Created_at { get; set;}
		public DateTime Expires_at { get; set; }
		public DateTime Accepted_at { get; set; }
		public string Invite_link { get; set; }
	}
}
