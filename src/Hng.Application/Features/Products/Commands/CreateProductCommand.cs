using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<CreateProductResponseDto>
    {
        public Guid OrgId { get; set; }
        public ProductCreationDto ProductDto { get; set; }

        public CreateProductCommand(Guid orgId, ProductCreationDto productDto)
        {
            OrgId = orgId;
            ProductDto = productDto;
        }
    }
}

