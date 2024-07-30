using AutoMapper;
using Hng.Application.Features.EmailTemplates.Commands;
using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.EmailTemplates.Handlers;

public class CreateEmailTemplateCommandHandler(IRepository<EmailTemplate> repository, IMapper mapper) : IRequestHandler<CreateEmailTemplateCommand, EmailTemplateDTO>
{
    private readonly IRepository<EmailTemplate> repository = repository;
    private readonly IMapper mapper = mapper;

    public async Task<EmailTemplateDTO> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
    {

        if (await repository.AnyAsync(e => e.Name.Equals(request.TemplateDTO.Name), cancellationToken)) return null;
        
        EmailTemplate template = mapper.Map<EmailTemplate>(request.TemplateDTO);

        template = await repository.AddAsync(template);

        await repository.SaveChanges();

        return mapper.Map<EmailTemplateDTO>(template);
    }
}
