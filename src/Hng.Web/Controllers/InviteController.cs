using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InviteController : ControllerBase
	{
		public InviteController() 
		{ 
		 
		
		}

		[HttpPost]
		public async Task<> Invite(InviteModel request)
		{
		}

		[HttpGet("accept")]
		public async Task<> Accept(string token)
		{
		}
	}
}
