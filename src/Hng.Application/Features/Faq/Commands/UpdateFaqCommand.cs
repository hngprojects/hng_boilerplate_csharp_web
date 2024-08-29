using Hng.Application.Features.Faq.Dtos;
using MediatR;

namespace Hng.Application.Features.Faq.Commands
{
    public class UpdateFaqCommand : IRequest<UpdateFaqResponseDto>
    {
        public UpdateFaqCommand(Guid id, UpdateFaqRequestDto faqRequest)
        {
            Id = id;
            FaqRequestDto = faqRequest;
        }
        public Guid Id { get; }
        public UpdateFaqRequestDto FaqRequestDto { get; }
    }
}

