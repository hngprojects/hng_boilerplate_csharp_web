using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Languages.Handlers.Connamds
{
    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, LanguageResponseDto>
    {
        private readonly IRepository<Language> _repository;

        public DeleteLanguageCommandHandler(IRepository<Language> repository)
        {
            _repository = repository;
        }

        public async Task<LanguageResponseDto> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
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

            await _repository.DeleteAsync(language);
            await _repository.SaveChanges();

            return new LanguageResponseDto
            {
                StatusCode = 200,
                Message = "Language deleted successfully"
            };
        }
    }
}