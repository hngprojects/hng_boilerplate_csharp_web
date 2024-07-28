using System.Text.Json.Serialization;

namespace Hng.Application.Features.SuperAdmin.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("crerated_on")]
        public DateTime CreatedAt { get; set; }
    }
}
