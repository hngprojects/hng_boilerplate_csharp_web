using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class OrganisationsController : ControllerBase
	{
		private readonly IOrganisationService _organisationService;

		public OrganisationsController(IOrganisationService organisationService)
		{
			_organisationService = organisationService;
		}
		
		[HttpPost("send-invite")]
		public async Task<IActionResult> SendInvite(OrganisationInviteRequestModel request)
		{
			var response = await _organisationService.SendInvite(request);
			return Ok(response);
			
		}

	}
}
