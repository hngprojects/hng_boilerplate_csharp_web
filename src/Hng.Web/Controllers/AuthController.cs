using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SignUpResponse>> UserSignUp([FromBody] UserSignUpDto body)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(new SignUpResponse
                {
                    Message = "Validation failed",
                });
            }

            var command = new UserSignUpCommand(body);
            var response = await _mediator.Send(command);

            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(UserSignUp), response);
        }
    }
}
