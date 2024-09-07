using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Notifications.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql.Features.Queries
{
    public partial class Queries
    {
        /// <summary>
        /// Retrieve user's notifications (Read + Unread)
        /// </summary>
        [Authorize]
        public async Task<GetNotificationsResponseDto> GetAllNotifications([FromServices] IMediator mediator)
        {
            var query = new GetAllNotificationsQuery();
            return await mediator.Send(query);
        }

        /// <summary>
        /// Retrieve user's notifications (Read or Unread) 
        /// </summary>
        [Authorize]
        public async Task<GetNotificationsResponseDto> GetNotifications(bool? is_read, [FromServices] IMediator mediator)
        {
            var query = new GetNotificationsQuery(is_read);
            return await mediator.Send(query);
        }
    }
}
