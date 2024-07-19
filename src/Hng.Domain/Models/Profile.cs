using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
    }
}