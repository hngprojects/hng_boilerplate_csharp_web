namespace Hng.Domain.Entities

{
    public class Profile : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}