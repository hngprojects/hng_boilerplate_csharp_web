using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities
{
    public class Language : EntityBase
    {
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}