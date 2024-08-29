using System.Text.Json.Serialization;

namespace Hng.Application.Features.Categories.Dtos
{
    public class CategoryListDto
    {
        [JsonPropertyName("categories")]
        public List<CategoryDto> Categories { get; set; }
    }
}