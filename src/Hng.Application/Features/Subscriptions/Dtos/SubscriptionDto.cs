using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Subscriptions.Dtos
{
    public class SubscriptionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("user_id")]
        public Guid? UserId { get; set; }
        [JsonPropertyName("organization_id")]
        public Guid? OrganizationId { get; set; }
        [JsonPropertyName("plan")]
        public SubscriptionPlan Plan { get; set; }
        [JsonPropertyName("frequency")]
        public SubscriptionFrequency Frequency { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }
    }
}
