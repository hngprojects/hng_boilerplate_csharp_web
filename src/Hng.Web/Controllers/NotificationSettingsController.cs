using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/settings")]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Notification Settings - User notification settings
        /// </summary>
        [HttpPost("notification-settings")]
        [ProducesResponseType(typeof(SuccessResponseDto<NotificationSettingsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNotificationSettings([FromBody] CreateNotificationSettingsDto command)
        {
            try
            {
                var createCommand = new CreateNotificationSettingsCommand(command);
                var response = await _mediator.Send(createCommand);
                return response != null
                    ? Ok(new SuccessResponseDto<NotificationSettingsDto> { Data = response })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = "User not found", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object> { Error = "Bad Request", Message = ex.Message, Data = false });
            }
        }
        /// <summary>
        /// Get Notification Settings by User ID
        /// </summary>
        [HttpGet("notification-settings/{user_id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<NotificationSettingsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotificationSettings(Guid user_id)
        {
            try
            {
                var query = new GetNotificationSettingsQuery(user_id);
                var response = await _mediator.Send(query);

                return response != null
                    ? Ok(new SuccessResponseDto<NotificationSettingsDto> { Data = response })
                    : NotFound(new FailureResponseDto<object>
                    {
                        Error = "Not Found",
                        Message = "Notification settings not found for the specified user",
                        Data = false
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }
    }
}


