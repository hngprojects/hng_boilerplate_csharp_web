

using Hng.Application.DTOs;

namespace Hng.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(CreateUserDTO model);
    }
}
