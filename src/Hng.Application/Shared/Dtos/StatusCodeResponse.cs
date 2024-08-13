using System.Text.Json.Serialization;

namespace Hng.Application.Shared.Dtos
{
    public class StatusCodeResponse<T>()
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; } = new { };
        //Workaround to prevent null data in the response when an object isn't passed
    }

}
