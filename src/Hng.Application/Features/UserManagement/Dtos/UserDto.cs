using System.Text.Json.Serialization;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Profiles.Dtos;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("profile")]
        public ProfileDto Profile { get; set; }

        [JsonIgnore]
        [JsonPropertyName("organisation")]
        public IEnumerable<OrganizationDto> Organizations { get; set; }

        [JsonIgnore]
        [JsonPropertyName("products")]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}