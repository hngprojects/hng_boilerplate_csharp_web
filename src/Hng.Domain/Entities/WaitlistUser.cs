using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class WaitlistUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
