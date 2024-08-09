using Hng.Application.Features.Products.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
    public class GetUserProductsQuery : IRequest<PagedListDto<ProductDto>>
    {
        public GetUserProductsQuery(GetProductsQueryParameters parameters)
        {
            productsQueryParameters = parameters;
        }

        public GetProductsQueryParameters productsQueryParameters { get; set; }
    }
}
