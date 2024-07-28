using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Queries
{
    public class GetUsersBySearchQuery : IRequest<PagedListDto<UserDto>>
    {
        public GetUsersBySearchQuery(UsersQueryParameters parameters)
        {
            usersQueryParameters = parameters;
        }

        public UsersQueryParameters usersQueryParameters { get; set; }
    }
}
