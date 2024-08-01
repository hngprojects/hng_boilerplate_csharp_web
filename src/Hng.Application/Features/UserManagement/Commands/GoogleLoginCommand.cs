using Hng.Application.Features.UserManagement.Dtos;
using MediatR;


namespace Hng.Application.Features.UserManagement.Commands
{
    public class GoogleLoginCommand : IRequest<UserLoginResponseDto<object>>
    {
        public string IdToken { get; }

        public GoogleLoginCommand(string idToken)
        {
            IdToken = idToken;
        }
    }
}
