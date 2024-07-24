using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Products.Mappers
{
    internal class CategoryMapperProfile : AutoMapper.Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Parent_id, opt => opt.MapFrom(src => src.ParentId))
                .ReverseMap();
        }
    }
}
