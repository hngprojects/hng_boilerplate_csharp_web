using System.Text.Json.Serialization;

namespace Hng.Application.Features.EmailTemplates.DTOs;

public record EmailTemplateDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("template_body")]
    public string TemplateBody { get; set; }

    [JsonPropertyName("placeholders")]
    public Dictionary<string, string> Placeholders { get; set; }
}

