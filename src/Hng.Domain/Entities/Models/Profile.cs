
namespace Hng.Domain.Entities.Models
{
    public class Profile : BaseModel
    {
        public User? User {  get; set; }
        public long UserId {  get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
