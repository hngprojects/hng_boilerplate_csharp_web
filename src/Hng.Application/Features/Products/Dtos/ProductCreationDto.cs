using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Products.Dtos
{
    public class ProductCreationDto
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("size")]
        public string Size { get; set; }
    }

    public class AddMultipleProductDto
    {
        public List<ProductCreationDto> Products { get; set; }
    }
}
