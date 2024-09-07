using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Features.EmailTemplates.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>> GetAllTemplates(GetAllEmailTemplatesQuery query, [FromServices] IMediator mediator)
        {
            return await mediator.Send(query);
        }
    }
}
