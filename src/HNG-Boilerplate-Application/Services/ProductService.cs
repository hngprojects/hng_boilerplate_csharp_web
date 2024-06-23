using HNG_Boilerplate_Domain.Entities;
using HNG_Boilerplate_Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNG_Boilerplate_Application.Services
{
    public class ProductService : IProductService
    {
        public readonly IGenericRepository<Product> _repoProduct;

        public ProductService(IGenericRepository<Product> repoProduct)
        {
            _repoProduct = repoProduct;
        }
        public bool AddProduct(Product product)
        {
            var result = _repoProduct.Add(product);
            return result.Result;
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            var result = _repoProduct.GetAll();
            return result;
        }

        public Task<Product> GetProduct(Guid productId)
        {
            var result = _repoProduct.GetById(productId);
            return result;
        }
    }
}
