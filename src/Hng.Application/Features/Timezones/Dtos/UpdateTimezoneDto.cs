using System.Text.Json.Serialization;

namespace Hng.Application.Features.Timezones.Dtos
{
    public class UpdateTimezoneDto
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("gmt_offset")]
        public string GmtOffset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}