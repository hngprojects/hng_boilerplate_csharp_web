using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Shared.Dtos
{
    public class FailureResponseDto<T>
    {
        public T Data { get; set; }
        public string Error { get; set; }
        public string Message { get; set; } = "Request failed.";
    }
}