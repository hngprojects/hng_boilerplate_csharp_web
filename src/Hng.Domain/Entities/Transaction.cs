using Hng.Domain.Enums;

namespace Hng.Domain.Entities
{
    public class Transaction : EntityBase
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
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