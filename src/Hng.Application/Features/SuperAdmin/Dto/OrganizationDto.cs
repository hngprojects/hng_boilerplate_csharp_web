using System.Text.Json.Serialization;

namespace Hng.Application.Features.SuperAdmin.Dto;

public class OrganizationDto
{

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("slug")] public string Slug { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("industry")] public string Industry { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("country")] public string Country { get; set; }

    [JsonPropertyName("address")] public string Address { get; set; }

    [JsonPropertyName("state")] public string State { get; set; }

    [JsonPropertyName("is_active")] public bool IsActive { get; set; }

    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

}

