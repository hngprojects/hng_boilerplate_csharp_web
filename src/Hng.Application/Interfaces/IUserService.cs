using Hng.Application.Dto;
using Hng.Domain.Entities;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);

        Task<IEnumerable<UserDto>> GetAllUsersAsync();


        Task<(bool IsSuccess, string ErrorMessage)> IsEmailUniqueAsync(string email);
        Task<UserResponseDto> CreateUserAsync(UserSignupDto userSignupDto);
        string GenerateJwtToken(UserResponseDto user);

    }
}
