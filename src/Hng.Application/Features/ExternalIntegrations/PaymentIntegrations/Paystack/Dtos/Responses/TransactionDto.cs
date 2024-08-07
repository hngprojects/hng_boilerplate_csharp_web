using Hng.Domain.Enums;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SubscriptionId { get; set; }
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