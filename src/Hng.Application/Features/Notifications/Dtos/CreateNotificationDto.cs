using System.Text.Json.Serialization;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
