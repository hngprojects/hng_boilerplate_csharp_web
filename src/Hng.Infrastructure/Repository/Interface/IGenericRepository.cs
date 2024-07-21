using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> GetAsync(Guid id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<bool> Exists(Guid id);
    }
}