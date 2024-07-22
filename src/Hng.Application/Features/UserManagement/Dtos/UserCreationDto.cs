using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserCreationDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}