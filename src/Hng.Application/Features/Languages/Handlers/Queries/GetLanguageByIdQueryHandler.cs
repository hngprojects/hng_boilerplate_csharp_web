using AutoMapper;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Queries
{
    public class GetLanguageByIdQueryHandler : IRequestHandler<GetLanguageByIdQuery, LanguageResponseDto>
    {
        private readonly IRepository<Language> _repository;
        private readonly IMapper _mapper;

        public GetLanguageByIdQueryHandler(IRepository<Language> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LanguageResponseDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
        {
            var language = await _repository.GetAsync(request.Id);
            if (language == null)
            {
                return new LanguageResponseDto
                {
                    StatusCode = 404,
                    Message = "Language not found"
                };
            }

            var responseDto = _mapper.Map<LanguageDto>(language);
            return new LanguageResponseDto
            {
                Language = responseDto,
                StatusCode = 200,
                Message = "Language retrieved successfully"
            };
        }
    }
}