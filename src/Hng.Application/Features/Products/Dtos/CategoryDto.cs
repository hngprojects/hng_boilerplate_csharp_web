namespace Hng.Application.Features.Products.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Parent_id { get; set; }
    }
}