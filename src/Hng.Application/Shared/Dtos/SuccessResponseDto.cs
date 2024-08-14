using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Shared.Dtos
{
    public class SuccessResponseDto<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = "Request completed successfully.";
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = 200;
    }
}
