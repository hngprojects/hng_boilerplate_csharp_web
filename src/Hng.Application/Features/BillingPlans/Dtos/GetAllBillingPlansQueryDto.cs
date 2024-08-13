using System.Text.Json.Serialization;

namespace Hng.Application.Features.BillingPlans.Dtos
{
    public class GetAllBillingPlansQueryDto
    {
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;
    }
}