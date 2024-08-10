using Hng.Application.Features.Timezones.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Timezones.Commands
{
    public class CreateTimezoneCommand : IRequest<CreateTimezoneResponseDto>
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("gmt_offset")]
        public string GmtOffset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}