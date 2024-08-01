﻿using Hng.Application.Features.Notifications.Dtos;
using MediatR;

namespace Hng.Application.Features.Notifications.Queries
{
    public class GetNotificationSettingsQuery : IRequest<NotificationDto>
    {
        public Guid UserId { get; }

        public GetNotificationSettingsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
