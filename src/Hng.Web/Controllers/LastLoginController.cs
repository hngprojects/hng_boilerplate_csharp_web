using Hng.Application.Features.LastLoginUser.Dto;
using Hng.Application.Features.LastLoginUser.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    /// <summary>
    /// Controller responsible for handling the user's last login data.
    /// </summary>
    [ApiController]
    [Route("api/last-login")]
    public class LastLoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;

        public LastLoginController(IMediator mediator, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Retrieves the last login information for the specified user.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches the last login details.
        /// </remarks>
        /// <response code="200">Successfully retrieved the last login information.</response>
        /// <response code="404">If the last login information is not found.</response>
        /// <returns>Last login details of the user.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(LastLoginResponseDto<List<LastLoginDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLastLogin()
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var result = await _mediator.Send(new GetLastLoginQuery { UserId = userId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(new SuccessResponseDto<List<LastLoginDto>>()
            {
                Data = result.Data,
                Message = "success"
            });
        }
    }

}
