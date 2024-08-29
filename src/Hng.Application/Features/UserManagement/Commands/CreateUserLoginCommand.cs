using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class CreateUserLoginCommand : IRequest<UserLoginResponseDto<SignupResponseData>>
    {
        public CreateUserLoginCommand(UserLoginRequestDto loginRequest)
        {
            LoginRequestBody = loginRequest;
        }

        public UserLoginRequestDto LoginRequestBody { get; }
    }
}
