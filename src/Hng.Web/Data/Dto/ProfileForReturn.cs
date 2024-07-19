using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Web.Data.Dto
{
    public class ProfileForReturn
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
    }
}