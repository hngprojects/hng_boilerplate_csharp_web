using System.Text.Json.Serialization;

namespace Hng.Application.Features.Faq.Dtos
{
    public class UpdateFaqResponseDto
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public FaqData Data { get; set; }
    }
}
