namespace Hng.Domain.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string ParentId { get; set; }
    }
}
