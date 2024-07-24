using System;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new { error = "Invalid product ID" });
                }

                var query = new GetProductByIdQuery { Id = id };
                var product = await _mediator.Send(query);

                return Ok(new { product });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Product not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product with ID {ProductId}", id);
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }
    }
}
