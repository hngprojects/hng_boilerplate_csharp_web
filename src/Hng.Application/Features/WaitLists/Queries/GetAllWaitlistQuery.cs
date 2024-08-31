using Hng.Domain.Entities;
using MediatR;

namespace Hng.Application.Features.WaitLists.Queries
{
    public class GetAllWaitlistQuery : IRequest<List<Waitlist>>
    {
    }
}
