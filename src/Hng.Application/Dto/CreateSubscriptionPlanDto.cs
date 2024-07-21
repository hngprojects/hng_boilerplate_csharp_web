using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Dto
{
    public class CreateSubscriptionPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Duration { get; set; }
        public List<string> Features { get; set; }
    }
}

