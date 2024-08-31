using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Application.Features.ContactsUs.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<ContactResponse<List<ContactUsResponseDto>>> GetAllContactMessages([FromServices] IMediator mediator)
        {
            var query = new GetAllContactUsQuery();
            return await mediator.Send(query);
        }
    }
}