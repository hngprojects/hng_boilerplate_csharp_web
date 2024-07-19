namespace Hng.Domain.Models
{
    public class OrganisationUser: EntityBase
    {
        public Guid OrganisationId { get; set; }
        public Guid UserId { get; set; }

        public Organisation Organisation { get; set; }
        public User User { get; set; }
    }
}