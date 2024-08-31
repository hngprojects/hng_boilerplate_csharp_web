using AutoMapper;
using Hng.Application.Features.WaitLists.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.WaitLists.Handlers
{
    public class GetAllWaitlistQueryHandler : IRequestHandler<GetAllWaitlistQuery, List<Waitlist>>
    {
        private readonly IRepository<Waitlist> _waitlistRepository;
        private readonly IMapper _mapper;

        public GetAllWaitlistQueryHandler(IRepository<Waitlist> waitlistRepository, IMapper mapper)
        {
            _waitlistRepository = waitlistRepository;
            _mapper = mapper;
        }

        public async Task<List<Waitlist>> Handle(GetAllWaitlistQuery request, CancellationToken cancellationToken)
        {
            var list = await _waitlistRepository.GetAllAsync();
            var waitList = list.ToList();
            return waitList;
        }
    }
}
