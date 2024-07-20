
namespace Hng.Domain.Entities.Models
{
    public class UserOrganisation : BaseModel
    {
        public Organisation Organisation { get; set; }
        public string OrganisationId {  get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
