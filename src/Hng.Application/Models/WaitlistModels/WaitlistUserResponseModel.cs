using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Models.WaitlistModels
{
    public class WaitlistUserResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
    }
}
