using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Dto
{
    public class NewsLetterSubscriptionDto
    {
        [EmailAddress]
        public string email { get; set; }
    }
}
