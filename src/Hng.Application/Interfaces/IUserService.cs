using Hng.Application.Dto;
using Hng.Domain.Entities;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetUsersByStatusAsync(string org_id, string status, int page, int limit);
    }

}
