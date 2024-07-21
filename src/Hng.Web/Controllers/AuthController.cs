using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Hng.Web.Models;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Web.Controllers
{
    [ApiController] 
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserSignupDto userSignupDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(new { message = "Validation failed", error = ModelState, status_code = 422 });
            }

            var (isUnique, errorMessage) = await _userService.IsEmailUniqueAsync(userSignupDto.Email);
            if (!isUnique)
            {
                return BadRequest(new { message = errorMessage, error = "Email already exists", status_code = 400 });
            }

            try
            {
                var createdUser = await _userService.CreateUserAsync(userSignupDto);
                var token = _userService.GenerateJwtToken(createdUser);

                var response = new SignupResponseDto
                {
                    Message = "User registered successfully",
                    Data = new SignupResponseData
                    {
                        Token = token,
                        User = createdUser
                    }
                };

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user");
                return StatusCode(500, new { message = "An error occurred while processing your request", error = "Internal server error", status_code = 500 });
            }
        }
    }
}