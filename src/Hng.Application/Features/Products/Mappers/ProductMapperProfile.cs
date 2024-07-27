using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Products.Mappers
{
    public class ProductMapperProfile : AutoMapper.Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }

}