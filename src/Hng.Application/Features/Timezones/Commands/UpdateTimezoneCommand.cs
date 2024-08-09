using Hng.Application.Features.Timezones.Dtos;
using MediatR;

namespace Hng.Application.Features.Timezones.Commands
{
    public class UpdateTimezoneCommand : IRequest<TimezoneResponseDto>
    {
        public Guid Id { get; set; }
        public string Timezone { get; set; }
        public string GmtOffset { get; set; }
        public string Description { get; set; }
    }
}