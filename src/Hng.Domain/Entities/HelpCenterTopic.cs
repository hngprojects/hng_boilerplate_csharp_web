using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Entities
{
    public class HelpCenterTopic : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }

}
