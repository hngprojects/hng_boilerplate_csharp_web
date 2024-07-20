using AutoMapper;
using Hng.Application.Interfaces;
using Hng.Application.Services;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IEmailService emailService, TokenService tokenService, IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpGet("signin-token")]
        public async Task<IActionResult> RequestSignInToken([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Invalid email input", status_code = 400 });
            }
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { message = "Email not found", status_code = 400 });
            }

            var token = _tokenService.GenerateToken();
            _tokenService.StoreToken(email, token);
            try
            {
                await _emailService.SendEmailAsync(email, "Your Sign-In Token", $"Your sign-in token is {token}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to send sign-in token", status_code = 400 });
            }

            return Ok(new { message = $"Sign-in token sent to {email}", status_code = 200 });
        }
    }
}
