using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<SuccessResponseDto<CategoryDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }

        public CreateCategoryCommand(string name, string description, string slug)
        {
            Name = name;
            Description = description;
            Slug = slug;
        }
    }
}