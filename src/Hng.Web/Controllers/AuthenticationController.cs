using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto>> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            var response = await _mediator.Send(command);

            if (response == null || response.Data == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    error = "Invalid email or password.",
                    status_code = StatusCodes.Status401Unauthorized
                });
            }

            return Ok(response);
        }
    }
}
