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

        /// <summary>
        /// Retrieve user's notifications (Read + Unread)
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(SuccessResponseDto<GetNotificationsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var query = new GetAllNotificationsQuery();
                var response = await _mediator.Send(query);
                return Ok(new SuccessResponseDto<GetNotificationsResponseDto>
                {
                    Message = "Notifications retrieved successfully",
                    Data = response
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
        /// <summary>
        /// Retrieve user's notifications (Read or Unread) 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponseDto<GetNotificationsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetNotifications([FromQuery] bool? is_read)
        {
            try
            {
                var query = new GetNotificationsQuery(is_read);
                var response = await _mediator.Send(query);
                return Ok(new SuccessResponseDto<GetNotificationsResponseDto>
                {
                    Message = is_read.HasValue && is_read.Value == false ? "Unread notifications retrieved successfully" : "Notifications retrieved successfully",

                    Data = response
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