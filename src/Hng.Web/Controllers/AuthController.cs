using AutoMapper;
using Hng.Application.Dto;
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
        private readonly ITokenService _tokenService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
 

        public AuthController(IEmailService emailService, ITokenService tokenService, IConfiguration configuration, IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _configuration = configuration;
            _jwtService = jwtService;
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
            await _tokenService.StoreTokenAsync(email, token);
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

        [HttpPost("signin-token")]
        public async Task<IActionResult> VerifySignInToken([FromBody] VerifyTokenRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized(new { message = "Invalid token or email", status_code = 401 });
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !await _tokenService.ValidateTokenAsync(request.Email, request.Token))
            {
                return Unauthorized(new { message = "Invalid token or email", status_code = 401 });
            }

            // Generate JWT token
            var jwtToken = _jwtService.GenerateToken(user);

            return Ok(new { message = "Sign-in successful", token = jwtToken, status_code = 200 });
        }
    }

  
}
