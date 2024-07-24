using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Enums;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Features.UserManagement.Dtos;
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

        /// <summary>
        /// Product Deletion - deletes a product owned by a specific user
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<UserDto>> DeleteProductById(Guid id)
        {
            var query = new DeleteProductByIdQuery(id);
            var response = await _mediator.Send(query);
            return response is ProductQueryStatusEnum.NotFound ? NotFound(new
            {
                message = "Product not found",
                status_code = 404
            }) : NoContent();
        }

        /// <summary>
        /// Product Categories - gets all categories for products
        /// </summary>
        /// <returns></returns>
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
