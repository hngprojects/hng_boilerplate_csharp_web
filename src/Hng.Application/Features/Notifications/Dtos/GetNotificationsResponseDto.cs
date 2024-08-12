using System.Text.Json.Serialization;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class GetNotificationsResponseDto
    {
        [JsonPropertyName("total_notification_count")]
        public int TotalNotificationCount { get; set; }
        [JsonPropertyName("total_unread_notification_count")]
        public int TotalUnreadNotificationCount { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }
}
