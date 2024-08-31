using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<UserDto> GetLoggedInUsersDetails([FromServices] IMediator mediator)
        {
            var query = new GetLoggedInUserDetailsQuery();
            return await mediator.Send(query);
        }


    }
}
