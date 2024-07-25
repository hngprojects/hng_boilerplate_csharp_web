using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class UserSignUpCommand : IRequest<SignUpResponse>
    {
        public UserSignUpCommand(UserSignUpDto signUpDto)
        {
            SignUpBody = signUpDto;
        }

        public UserSignUpDto SignUpBody { get; }
    }
}
