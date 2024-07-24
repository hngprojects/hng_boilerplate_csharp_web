using System.Text.Json.Serialization;
using Hng.Application.Features.UserManagement.Dtos;

namespace Hng.Application.Features.Organisations.Dtos;

public class OrganizationDto
{
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("industry")]
    public string Industry { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("owner_id")]
    public Guid OwnerId { get; set; }

    [JsonIgnore]
    [JsonPropertyName("users")]
    public ICollection<UserDto> Users { get; set; }
}