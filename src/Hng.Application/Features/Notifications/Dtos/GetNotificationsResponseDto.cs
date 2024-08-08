using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class GetNotificationsResponseDto
    {
        public int TotalNotificationCount { get; set; }
        public int TotalUnreadNotificationCount { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }
}
