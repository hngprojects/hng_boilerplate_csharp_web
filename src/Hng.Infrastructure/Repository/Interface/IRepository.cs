using System.Linq.Expressions;
using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetAsync(Guid id);
        Task<T> GetBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task<bool> Exists(Guid id);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task SaveChanges();
    }
}
