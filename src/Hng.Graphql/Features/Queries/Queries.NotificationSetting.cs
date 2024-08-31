using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Graphql
{
    public partial class Queries
    {
        /// <summary>
        /// Get Notification Settings by User ID
        /// </summary>
        [Authorize]
        public async Task<NotificationSettingsDto> GetNotificationSettings(Guid user_id, [FromServices] IMediator mediator)
        {
            var query = new GetNotificationSettingsQuery(user_id);
            return await mediator.Send(query);

        }
    }
}
