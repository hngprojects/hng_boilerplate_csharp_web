using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly MyDBContext myDBContext;

        public GenericRepository(MyDBContext myDBContext)
        {
            this.myDBContext = myDBContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await myDBContext.AddAsync(entity);
            await myDBContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var data = await GetAsync(id);
            if (data == null)
            {
                return null;
            }

            myDBContext.Set<T>().Remove(data);
            await myDBContext.SaveChangesAsync();

            return data;
        }

        public async Task<bool> Exists(Guid id)
        {
            var data = await GetAsync(id);
            return data != null;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await myDBContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetAsync(Guid id)
        {
            return await myDBContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            myDBContext.Set<T>().Update(entity);
            await myDBContext.SaveChangesAsync();
        }
    }
}