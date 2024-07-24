
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Responses
{
    public class ValidationException : Exception
    {
        public object Errors { get; set; }
    }
}
