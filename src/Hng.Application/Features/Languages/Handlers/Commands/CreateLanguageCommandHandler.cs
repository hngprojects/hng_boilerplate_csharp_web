using AutoMapper;
using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Connamds
{
    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, SuccessResponseDto<LanguageDto>>
    {
        private readonly IRepository<Language> _repository;
        private readonly IMapper _mapper;

        public CreateLanguageCommandHandler(IRepository<Language> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<LanguageDto>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            var existingLanguage = await _repository.GetBySpec(l => l.Name == request.Name);
            if (existingLanguage != null)
            {
                throw new ApplicationException("Language already exists");
            }

            var language = _mapper.Map<Language>(request);
            language.Id = Guid.NewGuid();
            await _repository.AddAsync(language);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<LanguageDto>(language);
            return new SuccessResponseDto<LanguageDto>
            {
                Data = responseDto,
                StatusCode = 201,
                Message = "Language created successfully"
            };
        }
    }
}