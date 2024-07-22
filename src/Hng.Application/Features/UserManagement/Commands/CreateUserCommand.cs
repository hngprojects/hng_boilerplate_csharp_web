using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserCommand(UserCreationDto creationDto)
        {
            UserBody = creationDto;
        }

        public UserCreationDto UserBody { get; }
    }
}