namespace Hng.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public  Guid OrganizationId { get; set; }
        public Organization Organisation { get; set; }
        public ICollection<RolePermission> Permissions { get; set; } = [];
        public ICollection<UserRole> UsersRoles { get; set; } = [];
    }
}
