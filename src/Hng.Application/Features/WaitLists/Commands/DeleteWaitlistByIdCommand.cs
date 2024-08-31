using MediatR;
using Hng.Domain.Entities;

namespace Hng.Application.Features.WaitLists.Commands
{
    public class DeleteWaitlistByIdCommand : IRequest<Waitlist>
    {
        public DeleteWaitlistByIdCommand(Guid id)
        {
            WaitListId = id;
        }

        public Guid WaitListId { get; }
    }
}
