using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        /// <summary>
        /// Notification Settings - User notification settings
        /// </summary>
        [Authorize]
        public async Task<NotificationSettingsDto> CreateNotificationSettings([FromBody] CreateNotificationSettingsDto command, [FromServices] IMediator mediator)
        {
            {
                var createCommand = new CreateNotificationSettingsCommand(command);
                return await mediator.Send(createCommand);
            }
        }
    }
}
