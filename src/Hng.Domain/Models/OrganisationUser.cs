using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Domain.Models
{
    public class OrganisationUser
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int UserId { get; set; }

        public Organisation Organisation { get; set; }
        public User User { get; set; }
    }
}