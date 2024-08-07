using Hng.Application.Features.Faq.Dtos;
using MediatR;
using System;

namespace Hng.Application.Features.Faq.Commands
{
    public class DeleteFaqCommand : IRequest<DeleteFaqResponseDto>
    {
        public DeleteFaqCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}

