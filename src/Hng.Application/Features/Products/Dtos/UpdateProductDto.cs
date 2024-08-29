using System.Text.Json.Serialization;

namespace Hng.Application.Features.Products.Dtos
{
    public class UpdateProductDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
