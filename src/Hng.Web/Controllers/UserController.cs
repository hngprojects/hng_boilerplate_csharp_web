using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var response = await _mediator.Send(query);
            return response is null ? NotFound(new
            {
                message = "User not found",
                is_successful = false,
                status_code = 404
            }) : Ok(response);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreationDto body)
        {
            var command = new CreateUserCommand(body);
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateUser), response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserLoginResponseDto>> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            var response = await _mediator.Send(command);

            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}