using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserLoginResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public UserDto Data { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
