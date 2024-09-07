using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql.Features.Mutations
{
    public partial class Mutations
    {
        /// <summary>
        /// Notification Settings - Create User notification 
        /// </summary>
        [Authorize]
        public async Task<NotificationResult> CreateNotification(CreateNotificationDto command, [FromServices] IMediator mediator)
        {
            var createCommand = new CreateNotificationCommand(command);
            return await mediator.Send(createCommand);
        }


        /// <summary>
        /// Mark a single notification as read
        /// </summary>
        /// <param name="notification_id">The ID of the notification</param>
        /// <param name="request">The request body containing is_read flag</param>
        /// <returns>Response indicating the result of the operation</returns>
        [Authorize]
        public async Task<NotificationDto> MarkNotificationAsRead(Guid notification_id, UpdateNotificationDto request, [FromServices] IMediator mediator)
        {
            var command = new UpdateNotificationCommand(notification_id, request.IsRead);
            return await mediator.Send(command);
        }

        /// <summary>
        /// Mark all notification as read
        /// </summary>
        /// <param name="request">The request body containing is_read flag</param>
        /// <returns>Response indicating the result of the operation</returns>
        [Authorize]
        public async Task<List<NotificationDto>> MarkAllNotificationAsRead(UpdateNotificationDto request, [FromServices] IMediator mediator)
        {
            var command = new MarkAllCommand(request.IsRead);
            return await mediator.Send(command);

        }

        /// <summary>
        /// Clear all user's notifications (Read or Unread) 
        /// </summary>
        [Authorize]
        public async Task<bool> DeleteAllNotifications([FromServices] IMediator mediator)
        {
            var command = new DeleteAllNotificationsCommand();
            return await mediator.Send(command);

        }

        /// <summary>
        /// Clear notification (Read or Unread) 
        /// </summary>
        [Authorize]
        public async Task<bool> DeleteNotificationById(Guid notification_id, [FromServices] IMediator mediator)
        {
            var command = new DeleteNotificationByIdCommand(notification_id);
            return await mediator.Send(command);
        }
    }
}
