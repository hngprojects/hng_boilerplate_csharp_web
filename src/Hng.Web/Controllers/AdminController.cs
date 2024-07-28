using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Admin: Users - gets all registered users by search parameters
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUsersBySearch([FromQuery] UsersQueryParameters parameters)
        {
            var users = await _mediator.Send(new GetUsersBySearchQuery(parameters));
            return Ok(new
            {
                status_code = 200,
                users.MetaData,
                users
            });
        }
    }
}
