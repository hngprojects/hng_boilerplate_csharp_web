using System.Text.Json.Serialization;

namespace Hng.Application.Features.Languages.Dtos
{
    public class LanguageDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}