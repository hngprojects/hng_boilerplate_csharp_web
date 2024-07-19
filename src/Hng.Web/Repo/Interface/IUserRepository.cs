using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Domain.Models;

namespace Hng.Web.Repo.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<object> GetUserById(int id);
    }
}