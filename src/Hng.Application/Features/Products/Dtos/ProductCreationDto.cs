using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Dtos
{
    public class ProductCreationDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }
    }
}
