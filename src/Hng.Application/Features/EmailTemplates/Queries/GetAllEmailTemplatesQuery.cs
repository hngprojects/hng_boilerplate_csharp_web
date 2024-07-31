using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.EmailTemplates.Queries;

public record GetAllEmailTemplatesQuery(int PageNumber = 1, int PageSize = 5) : IRequest<PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>>;
