using Hng.Application.Interfaces;
using Hng.Application.Models;
using Hng.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]

        public IActionResult SearchProducts([FromQuery] SearchParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => new { parameter = e.ErrorMessage.Split(' ')[0], message = e.ErrorMessage })),
                    statusCode = 422
                });
            }

            try
            {
                var products = _productService.SearchProducts(parameters);

                if (!products.Any())
                {
                    return Ok(new
                    {
                        success = true,
                        products = new List<Product>(),
                        statusCode = 204
                    });
                }

                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    products,
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server Error",
                    statusCode = 500
                });
            }
        }
    }
}

