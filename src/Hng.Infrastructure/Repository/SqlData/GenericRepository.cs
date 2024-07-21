using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository.SqlData
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly MyDBContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MyDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
