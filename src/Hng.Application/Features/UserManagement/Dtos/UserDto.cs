using System.Text.Json.Serialization;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserDto
    {
        [JsonPropertyName("fullname")]
        public string FullName { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("profile")]
        public ProfileDto Profile { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("organisations")]
        public IEnumerable<OrganizationDto> Organizations { get; set; }

        [JsonIgnore]
        [JsonPropertyName("products")]
        public IEnumerable<ProductDto> Products { get; set; }

        [JsonPropertyName("blogs")]
        public ICollection<Blog> Blogs { get; set; } = [];
    }
}