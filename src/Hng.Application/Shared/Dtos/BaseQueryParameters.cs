using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos
{
    public class BaseQueryParameters
    {
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; } = 1;
    }
}
