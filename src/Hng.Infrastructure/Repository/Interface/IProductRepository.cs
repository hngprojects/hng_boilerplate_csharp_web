using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdAsync(Guid id);
        void Update(Product product);
    }
}
