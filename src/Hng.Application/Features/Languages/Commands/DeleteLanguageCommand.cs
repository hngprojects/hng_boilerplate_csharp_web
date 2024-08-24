using Hng.Application.Features.Languages.Dtos;
using MediatR;

namespace Hng.Application.Features.Languages.Commands
{
    public class DeleteLanguageCommand : IRequest<LanguageResponseDto>
    {
        public Guid Id { get; set; }
    }
}