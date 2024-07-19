using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Domain.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 

        public ICollection<OrganisationUser> OrganisationUsers { get; set; }
    }
}