using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Dto
{
    public class SubscriptionPlanResponse : EntityBase
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public List<string> Features { get; set; }
    }
}
