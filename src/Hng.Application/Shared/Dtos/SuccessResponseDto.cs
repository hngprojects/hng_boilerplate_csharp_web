using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Shared.Dtos
{
    public class SuccessResponseDto<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = "Request completed successfully.";
    }
}
