using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.ApiStatuses.Dtos.Requests
{
    public class GetAllApiStatusesQueries : IRequest<PaginatedResponseDto<List<ApiStatusResponseDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
