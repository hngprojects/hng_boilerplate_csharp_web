using Hng.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductCommand(string userId, ProductCreationDto productCreation)
        {
            UserId = userId;
            productBody = productCreation;
        }

        public string UserId { get; }
        public ProductCreationDto productBody { get; }
    }
}
