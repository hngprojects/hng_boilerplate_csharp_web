using Hng.Domain.Entities;
using Hng.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IProductService
    {
        PaginatedResult<Product> SearchProducts(SearchParameters parameters, int page, int pageSize);

    }
}
