using Hng.Domain.Enums;

namespace Hng.Domain.Entities
{
    public class ApiStatus : EntityBase
    {
        public string ApiGroup { get; set; }

        public ApiStatusType Status { get; set; }

        public long ResponseTime { get; set; }

        public string Details { get; set; }

        public DateTime LastChecked { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}