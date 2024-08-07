using System.Text.Json.Serialization;

namespace Hng.Application.Features.Roles.Dto
{
    public class CreateRoleResponseDto
    {
        public int StatusCode { get; set; }
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

    }
}
