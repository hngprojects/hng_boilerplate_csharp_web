using System.Text.Json.Serialization;

namespace Hng.Application.Features.Roles.Dto
{
    public class CreateRoleResponseDto
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("data")]
        public RoleDetails Data { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

    }
}
