using AutoMapper;
using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Features.EmailTemplates.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.EmailTemplates.Handlers;

public class GetAllEmailTemplatesQueryHandler(IRepository<EmailTemplate> repository, IMapper mapper) : IRequestHandler<GetAllEmailTemplatesQuery, IEnumerable<EmailTemplateDTO>>
{
    private readonly IRepository<EmailTemplate> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<EmailTemplateDTO>> Handle(GetAllEmailTemplatesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<EmailTemplate> emailTemplates = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmailTemplateDTO>>(emailTemplates);
    }
}
