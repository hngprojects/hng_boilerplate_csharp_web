using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Products.Mappers
{
    internal class CategoryMapperProfile : AutoMapper.Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
