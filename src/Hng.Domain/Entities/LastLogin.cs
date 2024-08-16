using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class LastLogin : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        public DateTime? LogoutTime { get; set; }
        public string IPAddress { get; set; }
    }
}
