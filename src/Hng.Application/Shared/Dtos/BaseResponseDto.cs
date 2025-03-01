using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos
{
    public class BaseResponseDto<T>
    {
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(3)]
        public T Data { get; set; }
        [JsonPropertyName("message")]
        [JsonPropertyOrder(2)]
        public string Message { get; set; } = "Request completed successfully.";
        [JsonPropertyName("status_code")]
        [JsonPropertyOrder(1)]
        public int StatusCode { get; set; } = 200;
    }
}
