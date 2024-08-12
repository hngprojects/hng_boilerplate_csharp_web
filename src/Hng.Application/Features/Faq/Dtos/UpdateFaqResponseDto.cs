using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Faq.Dtos
{
    public class UpdateFaqResponseDto
    {
        public int StatusCode { get; set; }
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
        [JsonPropertyName("category")]
        public string category { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("updatedat")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("createdby")]
        public string CreatedBy { get; set; } = "SuperAdmin";
    }
}
