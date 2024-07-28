using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos
{
    public class BaseQueryParameters
    {
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }
    }
}
