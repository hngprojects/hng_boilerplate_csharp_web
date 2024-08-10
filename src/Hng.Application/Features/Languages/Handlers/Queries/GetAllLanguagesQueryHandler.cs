using AutoMapper;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Queries
{
    public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, IEnumerable<LanguageDto>>
    {
        private readonly IRepository<Language> _languageRepository;
        private readonly IMapper _mapper;

        public GetAllLanguagesQueryHandler(IMapper mapper, IRepository<Language> languageRepository)
        {
            _mapper = mapper;
            _languageRepository = languageRepository;
        }

        public async Task<IEnumerable<LanguageDto>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = await _languageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LanguageDto>>(languages);
        }
    }
}
