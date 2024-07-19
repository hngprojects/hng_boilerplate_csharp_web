using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Infrastructure.Context;
using Hng.Web.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyDBContext myDBContext;

        public GenericRepository(MyDBContext myDBContext)
        {
            this.myDBContext = myDBContext;
        }
        public async Task<object> AddAsync(T entity)
        {
            await myDBContext.AddAsync(entity);
            await myDBContext.SaveChangesAsync();

            return entity;
        }

        public async Task<object> DeleteAsync(int id)
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

        public async Task<bool> Exists(int id)
        {
            var data = await GetAsync(id);
            return data != null;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await myDBContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetAsync(int id)
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