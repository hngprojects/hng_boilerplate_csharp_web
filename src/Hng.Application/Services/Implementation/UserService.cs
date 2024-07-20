

using Hng.Application.DTOs;
using Hng.Application.Services.Interfaces;
using Hng.Domain.Entities.Models;
using Hng.Infrastructure.Context;

namespace Hng.Application.Services.Implementation
{
    public class UserService(MyDBContext context) :IUserService
    {
        public async Task<bool> CreateUser(CreateUserDTO model)
        {
            Dictionary<string, string> errors = new();
            if (string.IsNullOrEmpty(model.Email))
            {
                errors.Add("email", "This field is required");
            }
            if (errors.Count > 0)  return false;
            var newUser = new User
            {
                UserName = model.Email.ToLower(),
                Email = model.Email.Trim().ToLower()
            };
            await context.AddAsync(newUser);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
