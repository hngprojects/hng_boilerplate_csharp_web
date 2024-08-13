using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(Guid id)
        {
            UserId = id;
        }

        public Guid UserId { get; }
    }
}