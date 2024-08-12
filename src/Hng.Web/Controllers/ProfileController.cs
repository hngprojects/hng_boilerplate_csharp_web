using Hng.Application.Features.Profiles.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpPut("{email}")]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfileResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto profileDto, string email)
        {
            profileDto.Email = email;
            var response = await _mediator.Send(profileDto);

            if (response.IsFailure)
                return StatusCode(404, new UpdateProfileResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }

        [Authorize]
        [HttpPut("{userId}/picture")]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateProfilePictureResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureDto profileDto, Guid userId)
        {
            profileDto.UserId = userId;
            var response = await _mediator.Send(profileDto);

            if (response.IsFailure)
                return StatusCode(400,
                new UpdateProfilePictureResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }
    }
}
