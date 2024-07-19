using Hng.Application.Dto;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);
    }
}
