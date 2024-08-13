using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Dtos
{
    public class HelpCenterTopicResponseDto
    {
        public Guid Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
    }
}
