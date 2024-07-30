﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("mobile_push_notifications")]
        public bool MobilePushNotifications { get; set; }
        [JsonPropertyName("activity_workspace_email")]
        public bool ActivityWorkspaceEmail { get; set; }
        [JsonPropertyName("email_notifications")]
        public bool EmailNotifications { get; set; }
        [JsonPropertyName("email_digests")]
        public bool EmailDigests { get; set; }
        [JsonPropertyName("announcements_update_emails")]
        public bool AnnouncementsUpdateEmails { get; set; }
        [JsonPropertyName("activity_workspace_slack")]
        public bool ActivityWorkspaceSlack { get; set; }
        [JsonPropertyName("slack_notifications")]
        public bool SlackNotifications { get; set; }
        [JsonPropertyName("announcements_update_slack")]
        public bool AnnouncementsUpdateSlack { get; set; }
    }
}
