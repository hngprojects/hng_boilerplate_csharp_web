using Hng.Application.Features.LastLoginUser.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.LastLoginUser.Queries
{
    public class GetLastLoginQuery : IRequest<List<LastLoginDto>>
    {
        public Guid UserId { get; set; }
    }
}
