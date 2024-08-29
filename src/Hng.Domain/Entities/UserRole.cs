namespace Hng.Domain.Entities
{
    public class UserRole : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid OrganizationId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
        public Organization Orgainzation { get; set; }

    }
}
