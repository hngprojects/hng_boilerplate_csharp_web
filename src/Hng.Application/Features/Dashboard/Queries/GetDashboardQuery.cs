using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Dashboard.Queries
{
    public class GetDashboardQuery : IRequest<DashboardDto>
    {
        public Guid UserId { get; set; }

        public GetDashboardQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
