using System.Text.Json.Serialization;

namespace Hng.Application.Features.Faq.Dtos
{
    public class DeleteFaqResponseDto
    {
        public int StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

}
