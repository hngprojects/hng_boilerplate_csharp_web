using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }
    }
}
