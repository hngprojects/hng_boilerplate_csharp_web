using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
	public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>;
}
