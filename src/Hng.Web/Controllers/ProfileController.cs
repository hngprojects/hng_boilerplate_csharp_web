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
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{email}")]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto, string email)
        {
            var response = await _mediator.Send(new UpdateProfile(email, profileDto));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound,
                new UpdateProfileResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// update profile picture
        /// </summary>
        /// <param name="profileDto"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{email}/picture")]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePicture profileDto, string email)
        {
            var response = await _mediator.Send(new UpdateProfilePictureDto(email, profileDto.DisplayPhoto));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new UpdateProfilePictureResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }

        /// <summary>
        /// delete profile picture
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete("{email}/picture")]
        [ProducesResponseType(typeof(DeleteProfilePictureResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteProfilePictureResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProfilePicture(string email)
        {
            var response = await _mediator.Send(new DeleteProfilePictureDto() { Email = email });

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new DeleteProfilePictureResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }
    }
}