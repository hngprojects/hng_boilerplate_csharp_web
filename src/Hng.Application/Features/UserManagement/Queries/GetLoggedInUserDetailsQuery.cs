using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Queries
{
    public class GetLoggedInUserDetailsQuery : IRequest<UserDto>
    {

    }
}
