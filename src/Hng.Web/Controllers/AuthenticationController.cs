using CSharpFunctionalExtensions;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Logs in User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<object>>> Login([FromBody] UserLoginRequestDto loginRequest)
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

        /// <summary>
        /// Creates User
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SignUpResponse>> UserSignUp([FromBody] UserSignUpDto body)
        {
            var command = new UserSignUpCommand(body);
            var response = await _mediator.Send(command);

            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(UserSignUp), response);
        }

        /// <summary>
        /// sign in via google
        /// </summary>
        /// <param name="googleLoginRequest"></param>
        /// <returns></returns>
        [HttpPost("google")]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<object>>> GoogleLogin([FromBody] GoogleLoginRequestDto googleLoginRequest)
        {
            var command = new GoogleLoginCommand(googleLoginRequest.IdToken);
            var response = await _mediator.Send(command);

            if (response == null || response.Data == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    error = "Google login failed.",
                    status_code = StatusCodes.Status401Unauthorized
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Password Endpoint
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update/password")]
        [ProducesResponseType(typeof(Result<ChangePasswordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
                return BadRequest(response.Error);

            return Ok(response.Value);
        }
    }
}
