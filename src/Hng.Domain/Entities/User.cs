namespace Hng.Domain.Entities;

    public class User: EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Profile Profile { get; set; }
        public ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
