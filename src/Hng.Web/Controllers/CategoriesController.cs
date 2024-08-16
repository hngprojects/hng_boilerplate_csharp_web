using Hng.Application.Features.Categories.Commands;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="createCategoryDto">The category details</param>
        /// <returns>The created category</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponseDto<CategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<CategoryDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponseDto<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var command = new CreateCategoryCommand(
                createCategoryDto.Name,
                createCategoryDto.Description,
                createCategoryDto.Slug
            );
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns>The requested category</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuccessResponseDto<CategoryDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<CategoryDto>>> GetCategory(Guid id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all categories
        /// </summary>
        /// <returns>A list of all categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponseDto<List<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResponseDto<List<CategoryDto>>>> GetAllCategories([FromQuery] GetAllCategoriesQueryParams queryParams)
        {
            var query = new GetAllCategoriesQuery(queryParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuccessResponseDto<bool>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<bool>>> DeleteCategory(Guid id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}