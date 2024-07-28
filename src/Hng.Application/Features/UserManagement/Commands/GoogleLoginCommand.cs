using Hng.Application.Features.UserManagement.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class GoogleLoginCommand : IRequest<UserLoginResponseDto>
    {
        public string IdToken { get; set; }

        public GoogleLoginCommand(string idToken)
        {
            IdToken = idToken;
        }
    }
}
