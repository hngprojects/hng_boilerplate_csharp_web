using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Dto
{
    public class VerifyTokenRequestDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
