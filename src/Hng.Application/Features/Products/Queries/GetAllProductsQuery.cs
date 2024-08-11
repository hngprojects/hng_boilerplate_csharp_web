using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductResponseDto>>
    {
        public Guid OrgId { get; }

        public GetAllProductsQuery(Guid orgId)
        {
            OrgId = orgId;
        }
    }
}
