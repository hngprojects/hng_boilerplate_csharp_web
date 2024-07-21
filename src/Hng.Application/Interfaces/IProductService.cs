using Hng.Application.Models;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
    public interface IProductService
    {
        PaginatedResult<Product> SearchProducts(SearchParameters parameters, int page, int pageSize);
    }
}
