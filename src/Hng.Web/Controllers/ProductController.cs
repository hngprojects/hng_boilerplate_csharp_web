using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Responses;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;
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
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<UserDto>> CreateProduct([FromBody] ProductCreationDto body)
        {
            try
            {
                var command = new CreateProductCommand(body);
                var response = await _mediator.Send(command);

                return CreatedAtAction(nameof(CreateProduct), new
                {
                    status = "success",
                    message = "Product created successfully",
                    data = response
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Bad request",
                    Message = "Invalid input data",
                    Errors = ex.Errors
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Status = "Unauthorized",
                    Message = "Unauthorized. Please log in."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Internal server error",
                    Message = "An unexpected error occurred"
                });
            }
            //var command = new CreateProductCommand(body);
            //var response = await _mediator.Send(command);
            //return CreatedAtAction(nameof(CreateProduct), response);
        }

    }
}
