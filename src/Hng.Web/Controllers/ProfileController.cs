using Hng.Application.Features.Profiles.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// update user's details
        /// </summary>
        /// <param name="profileDto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto)
        {
            var response = await _mediator.Send(new UpdateProfile() { UpdateProfileDto = profileDto });

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status401Unauthorized,
                new UpdateProfileResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status401Unauthorized
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// update profile picture
        /// </summary>
        /// <param name="profileDto"></param>
        /// <returns></returns>
        [HttpPut("picture")]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureDto profileDto)
        {
            var response = await _mediator.Send(profileDto);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status401Unauthorized,
                new UpdateProfilePictureResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status401Unauthorized
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// delete profile picture
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("picture")]
        [ProducesResponseType(typeof(DeleteProfilePictureResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteProfilePictureResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProfilePicture(DeleteProfilePictureDto request)
        {
            var response = await _mediator.Send(request);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status401Unauthorized,
                new DeleteProfilePictureResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status401Unauthorized
                });

            return Ok(response.Value);
        }
    }
}