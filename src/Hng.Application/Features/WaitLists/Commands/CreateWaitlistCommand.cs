using Hng.Application.Features.WaitLists.Dtos;
using Hng.Domain.Entities;
using MediatR;

namespace Hng.Application.Features.WaitLists.Commands
{
    public class CreateWaitlistCommand : IRequest<Waitlist>
    {
        public CreateWaitlistCommand(WaitListDto createWaitlist)
        {
            WaitlistEntry = createWaitlist;
        }
        public WaitListDto WaitlistEntry { get; }
    }
}