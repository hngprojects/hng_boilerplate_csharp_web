using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<PagedListDto<UserDto>> GetUsersBySearch(UsersQueryParameters parameters, [FromServices] IMediator mediator)
        {
            var users = new GetUsersBySearchQuery(parameters);
            return await mediator.Send(users);
        }
    }
}