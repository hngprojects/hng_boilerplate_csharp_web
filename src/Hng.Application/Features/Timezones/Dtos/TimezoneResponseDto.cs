using Newtonsoft.Json;

namespace Hng.Application.Features.Timezones.Dtos
{
    public class TimezoneResponseDto
    {
        [JsonProperty("timezone")]
        public TimezoneDto Timezone { get; set; }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}