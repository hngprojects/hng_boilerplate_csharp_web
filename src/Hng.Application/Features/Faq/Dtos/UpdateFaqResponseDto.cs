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
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public FaqData Data { get; set; }

        public class FaqData
        {
            [JsonPropertyName("id")]
            public Guid Id { get; set; }

            [JsonPropertyName("question")]
            public string Question { get; set; }

            [JsonPropertyName("answer")]
            public string Answer { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }

            [JsonPropertyName("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonPropertyName("updated_at")]
            public DateTime UpdatedAt { get; set; }

            [JsonPropertyName("created_by")]
            public string CreatedBy { get; set; } = "SuperAdmin";
        }
    }

}
