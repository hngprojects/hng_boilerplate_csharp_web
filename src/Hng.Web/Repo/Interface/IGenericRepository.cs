using Hng.Domain.Models;

namespace Hng.Web.Repo.Interface
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> GetAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<object> AddAsync(T entity);
        Task<object> DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> Exists(int id);
    }
}