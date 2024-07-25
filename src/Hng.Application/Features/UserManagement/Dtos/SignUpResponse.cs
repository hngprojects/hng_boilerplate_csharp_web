using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class SignUpResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        public SignupResponseData Data { get; set; }
    }

    public class SignupResponseData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        public UserResponseDto User { get; set; }
    }
}
