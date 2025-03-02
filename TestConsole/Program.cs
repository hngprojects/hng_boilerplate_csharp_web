using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;

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