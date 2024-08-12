namespace Hng.Application.Features.Notifications.Dtos
{
    public class GetNotificationsResponseDto
    {
        public int TotalNotificationCount { get; set; }
        public int TotalUnreadNotificationCount { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }
}
