using Newtonsoft.Json;

namespace Hng.Application.Features.Timezones.Dtos
{
    public class TimezoneDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("gmt_offset")]
        public string GmtOffset { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}