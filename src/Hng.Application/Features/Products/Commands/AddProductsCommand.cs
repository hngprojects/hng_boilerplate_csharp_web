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
        public AddProductsCommand(string userId, List<ProductCreationDto> productCreation)
        {
            UserId = userId;
            productBody = productCreation;
        }

        public string UserId { get; }
        public List<ProductCreationDto> productBody { get; }
    }
}
