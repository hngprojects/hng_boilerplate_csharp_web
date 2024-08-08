using Hng.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Commands
{
    public class AddProductsCommand : IRequest<ProductsDto>
    {
        public AddProductsCommand(List<ProductCreationDto> productCreation)
        {
            productBody = productCreation;
        }

        public List<ProductCreationDto> productBody { get; }
    }
}
