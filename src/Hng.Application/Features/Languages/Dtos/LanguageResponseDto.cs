using System.Text.Json.Serialization;

namespace Hng.Application.Features.Languages.Dtos
{
    public class LanguageResponseDto
    {
        [JsonPropertyName("language")]
        public LanguageDto Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}