using System.Text.Json.Serialization;

namespace Hng.Application.Features.Products.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }
    }
}