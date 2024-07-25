using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hng.Application.Features.Invite.Commands;
using Hng.Application.Features.Invite.Dtos;
using Hng.Application.Features.Invite.Queries;

namespace Hng.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InviteController(IMediator mediator) : ControllerBase
	{

        [HttpPost]
        //[ProducesResponseType(typeof(OrganizationInviteDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<OrganizationInviteDto>> CreateOrganizationIvite([FromBody] CreateOrganizationInviteDto body)
        {
            var command = new CreateOrganizationInviteCommand(body);
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(CreateOrganizationIvite), response);
        }

        
        [HttpGet("accept")]
        //[ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrganizationInviteDto>> Accept(string token)
        {
            var query = new GetOrganizationInviteQuery(id);
            var response = await mediator.Send(query);
            return response is null ? NotFound(new
            {
                message = "Invite not found",
                is_successful = false,
                status_code = 404
            }) : Ok(response);
        }
    }
}
