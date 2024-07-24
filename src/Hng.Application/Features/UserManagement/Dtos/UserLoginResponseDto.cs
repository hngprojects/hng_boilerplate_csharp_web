using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class UserLoginResponseDto
    {
        public string Message { get; set; }
        public UserDto Data { get; set; }
        public string access_token { get; set; }
    }
}
