using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using System.Security.Claims;
using System.Text;


using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Hng.Infrastructure.Context;

namespace Hng.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        private readonly MyDBContext _context;

        public UserService(IUserRepository userRepository, IMapper mapper,IConfiguration configuration, MyDBContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;

        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);

        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            
            return _mapper.Map<IEnumerable<UserDto>>(users);
            
        }









        public async Task<(bool IsSuccess, string ErrorMessage)> IsEmailUniqueAsync(string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return existingUser == null
                ? (true, string.Empty)
                : (false, "Email already exists");
        }

        public async Task<UserResponseDto> CreateUserAsync(UserSignupDto userSignupDto)
        {
            var user = new User
            {
                FirstName = userSignupDto.FirstName,
                LastName = userSignupDto.LastName,
                Email = userSignupDto.Email,
                PasswordHash = HashPassword(userSignupDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public string GenerateJwtToken(UserResponseDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
