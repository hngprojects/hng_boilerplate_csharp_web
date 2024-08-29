using System.Text.Json.Serialization;

namespace Hng.Application.Features.Subscriptions.Dtos.Responses
{
    public record SubscribeFreePlanResponse
    {
        [JsonPropertyName("subscription_id")]
        public Guid SubscriptionId { get; set; }

        [JsonPropertyName("user_id")]
        public Guid? UserId { get; set; }

        [JsonPropertyName("organization_id")]
        public Guid? OrganizationId { get; set; }

        [JsonPropertyName("plan")]
        public string Plan { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("frequency")]
        public string Frequency { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }
    }
}
