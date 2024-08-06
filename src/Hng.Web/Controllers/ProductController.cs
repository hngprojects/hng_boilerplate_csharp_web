using System.Security.Claims;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
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
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreationDto body)
        {
            var loggedInUserId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var command = new CreateProductCommand(loggedInUserId, body);
            var response = await _mediator.Send(command);

            var successResponse = new SuccessResponseDto<ProductDto>();
            successResponse.Data = response;
            successResponse.Message = "Product Successfully";
            return Ok(successResponse);
        }

        [HttpPost("AddProducts")]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductsDto>> AddProducts([FromBody] AddMultipleProductDto body)
        {
            var loggedInUserId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var command = new AddProductsCommand(loggedInUserId, body.Products);
            var response = await _mediator.Send(command);

            var successResponse = new SuccessResponseDto<ProductsDto>();
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
                return BadRequest(new { error = "Invalid product ID" });
            }

            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            return Ok(new
            {
                status_code = 200,
                product
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

        /// <summary>
        /// Edit user products with update timestamp
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            var command = new UpdateProductCommand(id, updateProductDto);
            var result = await _mediator.Send(command);
            return result != null
                ? Ok(result)
                : NotFound(new
                {
                    status_code = 404,
                    message = "Product not found",
                    error = "Not Found"
                });
        }

        /// <summary>
        /// Get all product endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProducts([FromQuery] GetProductsQueryParameters parameters)
        {
            var products = await _mediator.Send(new GetProductsQuery(parameters));
            return Ok(new PaginatedResponseDto<PagedListDto<ProductDto>> { Data = products, Metadata = products.MetaData });
        }
        /// <summary>
        /// Product - Search products by name
        /// </summary>
        [HttpGet("search")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByName([FromQuery] string product_name)
        {
            var query = new GetProductByNameQuery(product_name);
            var products = await _mediator.Send(query);
            return Ok(products);
        }
    }
}