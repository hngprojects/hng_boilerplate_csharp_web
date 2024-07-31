using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<PagedListDto<ProductDto>>
    {
        public GetProductsQuery(GetProductsQueryParameters parameters)
        {
            productsQueryParameters = parameters;
        }

        public GetProductsQueryParameters productsQueryParameters { get; set; }
    }
}
