using MediatR;
using Hng.Application.Features.Products.Dtos;

namespace Hng.Application.Features.Products.Queries
{
  public class GetProductByIdQuery : IRequest<ProductDto>
  {
    public Guid Id { get; set; }
  }
}
