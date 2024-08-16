using Hng.Application.Features.Categories.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Categories.Mappers
{
    public class CategoryMapperProfile : AutoMapper.Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}