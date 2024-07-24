using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Responses
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
    }
}
