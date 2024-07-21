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
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        [HttpGet("search")]
        public IActionResult SearchProducts([FromQuery] SearchParameters parameters, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => new
                    {
                        Parameter = e.ErrorMessage.Split(' ')[0],
                        Message = e.ErrorMessage
                    })
                    .ToList();

                return UnprocessableEntity(new
                {
                    Success = false,
                    Errors = errors,
                    StatusCode = 422
                });
            }

            try
            {
                var paginatedResult = _productService.SearchProducts(parameters, page, pageSize);

                if (paginatedResult.TotalItems == 0)
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = paginatedResult,
                        StatusCode = 204
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = paginatedResult,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for products");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Server Error",
                    StatusCode = 500
                });
            }
        }
    }
}


