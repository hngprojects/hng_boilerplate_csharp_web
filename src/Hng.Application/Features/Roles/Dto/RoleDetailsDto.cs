using System.Text.Json.Serialization;

namespace Hng.Application.Features.Roles.Dto
{
    public class RoleDetailsDto
    {
        public int StatusCode { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("permissions")]
        public List<PermissionDto> Permissions { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class PermissionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

}
