using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        public async Task<CreateFaqResponseDto> CreateFaq(CreateFaqRequestDto faqRequest, [FromServices] IMediator mediator)
        {
            var command = new CreateFaqCommand(faqRequest);
            return await mediator.Send(command);
        }

        public async Task<UpdateFaqResponseDto> UpdateFaq(Guid id, UpdateFaqRequestDto faqRequest, [FromServices] IMediator mediator)
        {
            var command = new UpdateFaqCommand(id, faqRequest);
            return await mediator.Send(command);
        }

        public async Task<DeleteFaqResponseDto> DeleteFaq(Guid id, [FromServices] IMediator mediator)
        {
            var command = new DeleteFaqCommand(id);
            return await mediator.Send(command);
        }
    }
}
