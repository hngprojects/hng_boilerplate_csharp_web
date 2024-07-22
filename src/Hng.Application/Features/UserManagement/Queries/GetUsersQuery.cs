using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}