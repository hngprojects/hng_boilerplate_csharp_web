using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserLoginResponseDto<T>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
