using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities.Models
{
    public class Organisation : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
