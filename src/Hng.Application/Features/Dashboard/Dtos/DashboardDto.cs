using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Dashboard.Dtos
{
    public class DashboardDto
    {
        [JsonPropertyName("revenue")]

        public decimal Revenue { get; set; }

        [JsonPropertyName("subscriptions")]
        public int Subscriptions { get; set; }

        [JsonPropertyName("sales")]
        public int Sales { get; set; }

        [JsonPropertyName("activeSubscription")]
        public int ActiveSubscription { get; set; }

        [JsonPropertyName("monthSales")]
        public IEnumerable<TransactionDto> MonthSales { get; set; }
    }
}
