using System.Linq.Expressions;
using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _context.Set<T>().AnyAsync(predicate, cancellationToken: cancellationToken);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public Task<T> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var entities = _context.Set<T>().AsNoTracking();
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }

            return await entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var entities = _context.Set<T>().Where(predicate).AsNoTracking();
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }

            return await entities.ToListAsync();
        }


        public IQueryable<T> GetQueryableBySpec(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(predicate);
        }
        public async Task<T> GetBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var entities = _context.Set<T>().Where(predicate).AsNoTracking();
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }

            return await entities.FirstOrDefaultAsync();
        }

        public Task UpdateAsync(T entity)
        {
            var updated = _context.Set<T>().Update(entity);
            return Task.FromResult(updated);
        }

        public async Task SaveChanges()
        {

            //Add newly registered user to the organisation if they were invited
            var newUsers = _context.ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

            if (newUsers.Count != 0)
            {
                var newUserEmails = newUsers.Select(u => u.Email).ToList();
                var pendingInvites = await _context.OrganizationInvites
                    .Where(e => e.Status == Domain.Enums.OrganizationInviteStatus.Pending
                             && newUserEmails.Contains(e.Email))
                    .ToListAsync();

                foreach (var invite in pendingInvites)
                {
                    var invitedUser = newUsers.FirstOrDefault(u => u.Email == invite.Email);
                    if (invitedUser != null)
                    {
                        var org = await _context.Organizations.FindAsync(invite.OrganizationId);
                        if (org != null)
                        {
                            org.Users.Add(invitedUser);
                            invite.Status = Domain.Enums.OrganizationInviteStatus.Accepted;
                            invite.AcceptedAt = DateTimeOffset.UtcNow;
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
