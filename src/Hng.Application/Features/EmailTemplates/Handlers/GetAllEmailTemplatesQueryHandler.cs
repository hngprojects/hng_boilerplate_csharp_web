using AutoMapper;
using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Features.EmailTemplates.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hng.Application.Features.EmailTemplates.Handlers;

public class GetAllEmailTemplatesQueryHandler(IRepository<EmailTemplate> repository, IMapper mapper) : IRequestHandler<GetAllEmailTemplatesQuery, PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>>
{
    private readonly IRepository<EmailTemplate> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>> Handle(GetAllEmailTemplatesQuery request, CancellationToken cancellationToken)
    {

        if (request.PageNumber < 1 || request.PageSize < 1) return null;

        int itemCount = await _repository.CountAsync();
        IList<EmailTemplate> emailTemplates = await _repository.GetQueryableBySpec(e => true)
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .ToListAsync();

        PagedListDto<EmailTemplateDTO> pagedResponse = new(_mapper.Map<List<EmailTemplateDTO>>(emailTemplates), itemCount, request.PageNumber, request.PageSize);
        return new PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>()
        {
            Metadata = pagedResponse.MetaData,
            Data = pagedResponse
        };
    }
}
