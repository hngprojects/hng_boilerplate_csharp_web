using System.Text.Json.Serialization;

namespace Hng.Application.Features.Roles.Dto
{
    public class RoleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }

}
