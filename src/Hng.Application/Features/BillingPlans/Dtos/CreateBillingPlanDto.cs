using Hng.Domain.Enums;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.BillingPlans.Dtos
{
    public class CreateBillingPlanDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("frequency")]
        public SubscriptionFrequency Frequency { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}