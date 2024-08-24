using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Languages.Queries
{
    public class GetAllLanguagesQuery : BaseQueryParameters, IRequest<PaginatedResponseDto<List<LanguageDto>>>
    {
    }
}