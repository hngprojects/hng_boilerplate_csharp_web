using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class RateLimit
    {
        public Guid Id { get; set; } 
        public string UserId { get; set; }
        public int RequestCount { get; set; }
        public DateTime LastRequest { get; set; }
    }
}
