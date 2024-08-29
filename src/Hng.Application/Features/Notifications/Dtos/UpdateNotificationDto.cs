using System;
using System.Text.Json.Serialization;
namespace Hng.Application.Features.Notifications.Dtos
{
    public class UpdateNotificationDto
    {
        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }
    }
}
