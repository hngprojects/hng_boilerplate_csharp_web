using Hng.Application.Features.Languages.Dtos;
using MediatR;

namespace Hng.Application.Features.Languages.Queries
{
    public class GetAllLanguagesQuery : IRequest<IEnumerable<LanguageDto>>
    {
    }
}