using AutoMapper;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql.Features.UserManagement
{
    public class UserManagementQueries
    {
        public async Task<UserDto> GetLoggedInUserdetails([FromServices] IAuthenticationService authService, [FromServices] IRepository<User> _userRepository, [FromServices] IMapper mapper) =>
            await new GetLoggedInUserDetailsQueryHandler(authService, _userRepository, mapper)
            .Handle(new GetLoggedInUserDetailsQuery(), CancellationToken.None);
    }
}
