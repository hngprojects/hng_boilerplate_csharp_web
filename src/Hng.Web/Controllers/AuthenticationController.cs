﻿using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Application.Shared.Dtos;
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

        public AuthenticationController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Gets Logged in User Details, Run This one, Server and Docs Response Differ
        /// </summary>
        [Authorize]
        [HttpGet("@me")]
        [ProducesResponseType(typeof(SuccessResponseDto<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessResponseDto<UserDto>>> GetLoggedInUsersDetails()
        {
            var query = new GetLoggedInUserDetailsQuery();
            var response = await _mediator.Send(query);

            return Ok(new SuccessResponseDto<UserDto> { Data = response, StatusCode = StatusCodes.Status200OK });
        }

        /// <summary>
        /// Logs in User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto<SignupResponseData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> Login([FromBody] UserLoginRequestDto loginRequest)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(UserLoginResponseDto<SignupResponseData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> GoogleLogin([FromBody] GoogleLoginRequestDto googleLoginRequest)
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
        [HttpPut("password")]
        [ProducesResponseType(typeof(ChangePasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ChangePasswordResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new ChangePasswordResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// forgot password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await _mediator.Send(new ForgotPasswordDto(request.Email, false));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound,
                new ForgotPasswordResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// forgot password for mobile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("forgot-password-mobile")]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPasswordMobile([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await _mediator.Send(new ForgotPasswordDto(request.Email, true));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound,
                new ForgotPasswordResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// verifies forgot password code for mobile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(VerifyForgotPasswordCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(VerifyForgotPasswordCodeResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyForgotPasswordCode([FromBody] VerifyForgotPasswordCodeDto request)
        {
            var response = await _mediator.Send(request);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new VerifyForgotPasswordCodeResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// resets password for mobile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("reset-password-mobile")]
        [ProducesResponseType(typeof(PasswordResetMobileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PasswordResetMobileResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PasswordResetMobile([FromBody] PasswordResetMobileDto request)
        {
            var response = await _mediator.Send(new PasswordResetMobileCommand(request));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound,
                new PasswordResetMobileResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// resets password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("reset-password")]
        [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDto request)
        {
            var response = await _mediator.Send(request);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new PasswordResetResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// Logs in User via Facebook
        /// </summary>
        /// <param name="request">The Facebook login request containing the access token.</param>
        /// <returns>A response with the login result or an error message.</returns>
        [HttpPost("facebook")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserLoginResponseDto<object>>> FacebookLogin([FromBody] FacebookLoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid request data",
                    error = "Invalid data provided.",
                    status_code = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                var command = new FacebookLoginCommand(request.AccessToken);
                var response = await _mediator.Send(command);

                if (response == null || response.Data == null)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid Facebook token",
                        error = "Unable to authenticate with Facebook.",
                        status_code = StatusCodes.Status401Unauthorized
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message,
                    status_code = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}