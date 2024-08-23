using AutoMapper;
using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Connamds
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, LanguageResponseDto>
    {
        private readonly IRepository<Language> _repository;
        private readonly IMapper _mapper;

        public UpdateLanguageCommandHandler(IRepository<Language> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LanguageResponseDto> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var existingLanguage = await _repository.GetAsync(request.Id);
            if (existingLanguage == null)
            {
                return new LanguageResponseDto
                {
                    StatusCode = 404,
                    Message = "Language not found"
                };
            }

            _mapper.Map(request, existingLanguage);
            await _repository.UpdateAsync(existingLanguage);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<LanguageDto>(existingLanguage);
            return new LanguageResponseDto
            {
                Language = responseDto,
                StatusCode = 200,
                Message = "Language updated successfully"
            };
        }
    }
}
