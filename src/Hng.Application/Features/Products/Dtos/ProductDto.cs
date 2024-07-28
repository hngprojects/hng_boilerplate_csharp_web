using Hng.Domain.Entities;
using System;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Products.Dtos
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("update_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        [JsonPropertyName("transactions")]
        public ICollection<Transaction> Transactions { get; set; }
    }
}