using Hng.Domain.Enums;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses
{
    public class TransactionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("product_id")]
        public Guid? ProductId { get; set; }

        [JsonPropertyName("subscription_id")]
        public Guid? SubscriptionId { get; set; }

        [JsonPropertyName("type")]
        public TransactionType Type { get; set; }

        [JsonPropertyName("status")]
        public TransactionStatus Status { get; set; }

        [JsonPropertyName("partners")]
        public TransactionIntegrationPartners Partners { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTime? PaidAt { get; set; }

        [JsonPropertyName("modified_at")]
        public DateTime? ModifiedAt { get; set; }
    }
}