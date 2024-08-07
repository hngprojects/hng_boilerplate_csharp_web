using Hng.Application.Features.Faq.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Faq.Commands
{
    public class CreateFaqCommand : IRequest<CreateFaqResponseDto>
    {
        public CreateFaqCommand(CreateFaqRequestDto faqRequest)
        {
            FaqRequestDto = faqRequest;
        }
        public CreateFaqRequestDto FaqRequestDto { get; }
    }
}
