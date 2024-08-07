using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Notification Settings - Create User notification 
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(typeof(SuccessResponseDto<NotificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto command)
        {
            try
            {
                var createCommand = new CreateNotificationCommand(command);
                var response = await _mediator.Send(createCommand);
                if (response.IsSuccess)
                {
                    return StatusCode(201, new SuccessResponseDto<NotificationDto>
                    {
                        Message = "Notification created successfully",
                        Data = response.Notification
                    });
                }
                return BadRequest(response.FailureResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object> { Error = "Bad Request", Message = ex.Message, Data = false });
            }
        }
    }
}