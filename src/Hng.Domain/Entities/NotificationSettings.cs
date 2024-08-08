namespace Hng.Domain.Entities
{
    public class NotificationSettings : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool MobilePushNotifications { get; set; }
        public bool ActivityWorkspaceEmail { get; set; }
        public bool EmailNotifications { get; set; }
        public bool EmailDigests { get; set; }
        public bool AnnouncementsUpdateEmails { get; set; }
        public bool ActivityWorkspaceSlack { get; set; }
        public bool SlackNotifications { get; set; }
        public bool AnnouncementsUpdateSlack { get; set; }
    }
}
