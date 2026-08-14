using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.NewsLetterSubscription.Dtos
{
    public class PaginatedSubscribersResponseDto
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<NewsLetterSubscriptionDto> Data { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }

    public class PaginationMetadata
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
    }
}
