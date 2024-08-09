using Hng.Application.Features.Timezones.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Timezones.Queries
{
    public class GetAllTimezonesQuery : IRequest<PaginatedResponseDto<List<TimezoneDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}