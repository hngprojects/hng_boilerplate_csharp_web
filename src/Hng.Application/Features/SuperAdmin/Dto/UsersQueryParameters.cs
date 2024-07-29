using Hng.Application.Shared.Dtos;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.SuperAdmin.Dto
{
    public class UsersQueryParameters : BaseQueryParameters
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("first_name")]
        public string Firstname { get; set; }
        [JsonPropertyName("last_name")]
        public string Lastname { get; set; }
    }
}
