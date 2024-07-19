
namespace Hng.Domain.Entities.Models
{
    public class UserOrganisation : BaseModel
    {
        public Organisation Organisation { get; set; }
        public long OrganisationId {  get; set; }
        public long UserId { get; set; }
    }
}
