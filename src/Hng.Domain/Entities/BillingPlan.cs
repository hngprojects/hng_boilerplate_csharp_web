using Hng.Domain.Enums;

namespace Hng.Domain.Entities
{
    public class BillingPlan : EntityBase
    {
        public string Name { get; set; }

        public SubscriptionFrequency Frequency { get; set; }

        public bool IsActive { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}