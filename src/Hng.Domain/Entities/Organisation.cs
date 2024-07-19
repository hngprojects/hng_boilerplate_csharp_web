namespace Hng.Domain.Entities;
    public class Organisation: EntityBase
    {
        public string Name { get; set; } 
        public string Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
}
