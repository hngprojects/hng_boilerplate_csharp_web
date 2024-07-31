using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserLoginRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
