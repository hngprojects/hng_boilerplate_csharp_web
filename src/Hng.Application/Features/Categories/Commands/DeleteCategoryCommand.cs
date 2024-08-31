using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Categories.Commands
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<SuccessResponseDto<bool>>
    {
    }
}