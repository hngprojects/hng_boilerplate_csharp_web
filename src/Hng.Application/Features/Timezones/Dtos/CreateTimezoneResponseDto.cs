using Newtonsoft.Json;

namespace Hng.Application.Features.Timezones.Dtos
{
    public class CreateTimezoneResponseDto
    {
        [JsonProperty("timezone")]
        public TimezoneDto Timezone { get; set; }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}