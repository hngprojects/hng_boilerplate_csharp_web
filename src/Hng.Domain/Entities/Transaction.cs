using Hng.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hng.Domain.Entities
{
    public class Transaction : EntityBase
    {
        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid? SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription Subscription { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public TransactionIntegrationPartners Partners { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}