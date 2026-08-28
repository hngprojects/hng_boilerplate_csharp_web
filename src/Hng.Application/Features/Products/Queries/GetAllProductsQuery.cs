using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductResponseDto>>
    {
        public Guid OrgId { get; }
        public string Category { get; }

        public GetAllProductsQuery(Guid orgId, string? category)
        {
            OrgId = orgId;
            Category = category;
        }
    }
}
