using Hng.Application.Features.Timezones.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Timezones.Commands
{
    public class UpdateTimezoneCommand : IRequest<TimezoneResponseDto>
    {
        public Guid Id { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("gmt_offset")]
        public string GmtOffset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}