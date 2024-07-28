using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.UserManagement.Dtos;
using System;
using Hng.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hng.Application.Shared.Dtos;

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

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreationDto body)
        {
            var command = new CreateProductCommand(body);
            var response = await _mediator.Send(command);

            var successResponse = new SuccessResponseDto<ProductDto>();
            successResponse.Data = response;
            successResponse.Message = "Product Successfully";
            return Ok(successResponse);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                var fail = new FailureResponseDto<ProductDto>
                {
                    Data = null,
                    Message = "Invalid product ID"
                };
                return BadRequest(fail);
            }

            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            return Ok(new SuccessResponseDto<ProductDto>()
            {
                Data = product,
                Message = "Successful"
            });
        }

        /// <summary>
        /// Product Deletion - deletes a product owned by a specific user
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            var command = new DeleteProductByIdCommand(id);
            var deletedProduct = await _mediator.Send(command);
            return deletedProduct is not null ? NoContent() : NotFound(new
            {
                status_code = 404,
                message = "Product not found"
            });
        }

        /// <summary>
        /// Product Categories - gets all categories for products
        /// </summary>
        [HttpGet("categories")]
        [Authorize]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GeProductCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(new
            {
                status_code = 200,
                categories
            });
        }
    }
}