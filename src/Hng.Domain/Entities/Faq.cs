using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class Faq : EntityBase
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string category { get; set; }
    }
}
