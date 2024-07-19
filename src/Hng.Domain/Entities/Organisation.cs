namespace Hng.Domain.Entities;
    public class Organisation: EntityBase
    {
        public string Name { get; set; } 
        public string Description { get; set; } 

        public ICollection<OrganisationUser> OrganisationUsers { get; set; }
    }
