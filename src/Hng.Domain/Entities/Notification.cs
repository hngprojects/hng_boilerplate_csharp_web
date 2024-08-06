using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class Notification : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool IsRead { get; set; } = false;
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
