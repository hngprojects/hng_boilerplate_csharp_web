using System.Text.Json.Serialization;

namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public class GetSubscriptionsQueryParameters
    {
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }
    }
}
