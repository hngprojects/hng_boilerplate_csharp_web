using System.Text.Json.Serialization;

namespace Hng.Application.Features.Languages.Dtos
{
    public class UpdateLanguageRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}