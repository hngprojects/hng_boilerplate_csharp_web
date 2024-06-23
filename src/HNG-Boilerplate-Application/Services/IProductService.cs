using HNG_Boilerplate_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNG_Boilerplate_Application.Services
{
    public interface IProductService
    {
        bool AddProduct(Product product);
        Task<Product> GetProduct(Guid productId);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
