using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hng.Domain.Models
{
    public class User: IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<OrganisationUser> OrganisationUsers { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}