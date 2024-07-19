using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ProfileId { get; set; }


        public Profile Profile { get; set; }
        public ICollection<OrganisationUser> OrganisationUsers { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}