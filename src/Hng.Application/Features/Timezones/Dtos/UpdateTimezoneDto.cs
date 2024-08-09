namespace Hng.Application.Features.Timezones.Dtos
{
    public class UpdateTimezoneDto
    {
        public string Timezone { get; set; }
        public string GmtOffset { get; set; }
        public string Description { get; set; }
    }
}