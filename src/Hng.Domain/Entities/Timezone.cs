namespace Hng.Domain.Entities
{
    public class Timezone : EntityBase
    {
        public string TimezoneValue { get; set; }
        public string GmtOffset { get; set; }
        public string Description { get; set; }
    }
}