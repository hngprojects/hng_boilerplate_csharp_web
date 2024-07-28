using Hng.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hng.Domain.Entities
{
    public class Subscription : EntityBase
    {
        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public Guid? TransactionId { get; set; }

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        public SubscriptionPlan Plan { get; set; }

        public SubscriptionFrequency Frequency { get; set; }

        public bool IsActive { get; set; }

        public decimal Amount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
