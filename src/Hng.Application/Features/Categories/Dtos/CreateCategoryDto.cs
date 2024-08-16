using System.Text.Json.Serialization;

namespace Hng.Application.Features.Categories.Dtos
{
    public class CreateCategoryDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }
}