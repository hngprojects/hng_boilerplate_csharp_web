using Hng.Application.Features.Languages.Dtos;
using MediatR;

namespace Hng.Application.Features.Languages.Queries
{
    public class GetLanguageByIdQuery : IRequest<LanguageResponseDto>
    {
        public Guid Id { get; set; }
    }
}