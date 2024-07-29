using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    }
}
