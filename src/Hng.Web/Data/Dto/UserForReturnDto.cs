using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Web.Data.Dto
{
    public class UserForReturnDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public ProfileForReturn Profile { get; set; }
        public List<OrganisationDto> Organisations { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}