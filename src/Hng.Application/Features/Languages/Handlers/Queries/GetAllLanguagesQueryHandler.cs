using AutoMapper;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Queries
{
    public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, PaginatedResponseDto<List<LanguageDto>>>
    {
        private readonly IRepository<Language> _languageRepository;
        private readonly IMapper _mapper;

        public GetAllLanguagesQueryHandler(IRepository<Language> languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponseDto<List<LanguageDto>>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = await _languageRepository.GetAllAsync();

            var paginatedLanguages = PagedListDto<LanguageDto>.ToPagedList(
                _mapper.Map<IEnumerable<LanguageDto>>(languages),
                request.Offset,
                request.Limit
            );

            return new PaginatedResponseDto<List<LanguageDto>>
            {
                Data = paginatedLanguages.ToList(),
                Metadata = paginatedLanguages.MetaData
            };
        }
    }
}