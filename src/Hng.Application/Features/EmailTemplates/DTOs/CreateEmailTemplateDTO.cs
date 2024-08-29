using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.EmailTemplates.DTOs;

public record CreateEmailTemplateDTO
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [Required]
    [JsonPropertyName("template_body")]
    public string TemplateBody { get; set; }

    [JsonPropertyName("placeholders")]
    public Dictionary<string, string> Placeholders { get; set; }
}

