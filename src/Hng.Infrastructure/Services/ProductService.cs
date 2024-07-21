using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Models;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        MyDBContext _context;

        public ProductService(MyDBContext context)
        {
            _context = context;
        }
        public PaginatedResult<Product> SearchProducts(SearchParameters parameters, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(parameters.Name))
                {
                    query = query.Where(p => EF.Functions.Like(p.Name, $"%{parameters.Name}%"));
                }

                if (!string.IsNullOrWhiteSpace(parameters.Category))
                {
                    query = query.Where(p => EF.Functions.Like(p.Category, parameters.Category));
                }

                if (parameters.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= parameters.MinPrice.Value);
                }

                if (parameters.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= parameters.MaxPrice.Value);
                }

                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var items = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PaginatedResult<Product>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };
            }
            catch (Exception)
            {
                // Log the exception here
                throw;
            }
        }
    }
}
