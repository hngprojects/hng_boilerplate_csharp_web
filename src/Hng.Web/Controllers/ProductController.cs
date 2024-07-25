using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            var validationResult = new UpdateProductDtoValidator().Validate(updateProductDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    status_code = 400,
                    message = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    error = "Bad Request"
                });
            }

            try
            {
                var command = new UpdateProductCommand(id, updateProductDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    status_code = 404,
                    message = new[] { ex.Message },
                    error = "Not Found"
                });
            }
        }
    }
}