using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class FacebookLoginCommand : IRequest<UserLoginResponseDto<object>>
    {
        public string FacebookToken { get; }

        public FacebookLoginCommand(string facebookToken)
        {
            FacebookToken = facebookToken;
        }
    }

}
