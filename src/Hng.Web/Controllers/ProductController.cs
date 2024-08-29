using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Queries;
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
    [Route("api/v1")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Product - Creates a new product
        /// </summary>
        [HttpPost("organisations/{orgId:guid}/products")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<CreateProductResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProduct(Guid orgId, [FromBody] ProductCreationDto createProductDto)
        {
            try
            {
                var command = new CreateProductCommand(orgId, createProductDto);
                var response = await _mediator.Send(command);
                return response != null
                    ? CreatedAtAction(nameof(CreateProduct), new SuccessResponseDto<CreateProductResponseDto> { Message = "Product created successfully", Data = response })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Organization with ID {orgId} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        /// <summary>
        /// Product - Search for products
        /// </summary>
        [HttpGet("organisations/{orgId:guid}/products")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllProducts(Guid orgId)
        {
            try
            {
                var query = new GetAllProductsQuery(orgId);
                var response = await _mediator.Send(query);
                return Ok(new SuccessResponseDto<IEnumerable<ProductResponseDto>>
                {
                    Message = "Products retrieved successfully",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        [HttpPost("products/add-products")]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductsDto>> AddProducts([FromBody] AddMultipleProductDto body)
        {
            var command = new AddProductsCommand(body.Products);
            var response = await _mediator.Send(command);

            var successResponse = new SuccessResponseDto<ProductsDto>();
            successResponse.Data = response;
            successResponse.Message = "Product Successfully";
            return Ok(successResponse);
        }

        [HttpGet("products/{id}")]
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
        [HttpDelete("products/{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            try
            {
                var command = new DeleteProductByIdCommand(id);
                var response = await _mediator.Send(command);
                return response != null
                    ? Ok(new SuccessResponseDto<object> { Message = "Product deleted successfully", Data = true })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Product with ID {id} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        /// <summary>
        /// Product Categories - gets all categories for products
        /// </summary>
        [HttpGet("products/categories")]
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
        [HttpPut("products/{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                var command = new UpdateProductCommand(id, updateProductDto);
                var response = await _mediator.Send(command);
                return response != null
                    ? Ok(new SuccessResponseDto<ProductDto> { Message = "Product updated successfully", Data = response })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Product with ID {id} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        /// <summary>
        /// Get all product endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
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
        [HttpGet("products/search")]
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

        /// <summary>
        /// Get all product endpoint with no search
        /// </summary>
        /// <returns></returns>
        [HttpGet("products/get-user-product")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserProduct([FromQuery] GetProductsQueryParameters parameters)
        {
            var products = await _mediator.Send(new GetUserProductsQuery(parameters));
            return Ok(new PaginatedResponseDto<PagedListDto<ProductDto>> { Data = products, Metadata = products.MetaData });
        }

    }
}
