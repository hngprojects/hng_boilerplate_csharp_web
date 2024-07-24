using System;
using System.Collections.Generic;
using System.Linq;

namespace Hng.Application.Features.Products.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
