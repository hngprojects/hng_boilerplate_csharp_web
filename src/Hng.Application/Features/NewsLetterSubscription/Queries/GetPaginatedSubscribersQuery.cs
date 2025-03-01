using Hng.Application.Features.NewsLetterSubscription.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.NewsLetterSubscription.Queries
{
    public class GetPaginatedSubscribersQuery : IRequest<PaginatedSubscribersResponseDto>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
