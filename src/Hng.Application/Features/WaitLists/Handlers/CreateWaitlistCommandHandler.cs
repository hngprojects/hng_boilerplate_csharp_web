using AutoMapper;
using Hng.Application.Features.WaitLists.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.WaitLists.Handlers
{
    public class CreateWaitlistCommandHandler : IRequestHandler<CreateWaitlistCommand, Waitlist>
    {
        private readonly IRepository<Waitlist> _waitlistRepository;
        private readonly IMapper _mapper;
        public CreateWaitlistCommandHandler(IRepository<Waitlist> waitlistRepository, IMapper mapper)
        {
            _waitlistRepository = waitlistRepository;
            _mapper = mapper;
        }
        public async Task<Waitlist> Handle(CreateWaitlistCommand request, CancellationToken cancellationToken)
        {
            var settings = await _waitlistRepository.GetBySpec(u => u.Email == request.WaitlistEntry.Email);
            if (settings != null)
            {
                return null;
            }
            var list = _mapper.Map<Waitlist>(request.WaitlistEntry);
            await _waitlistRepository.AddAsync(list);
            await _waitlistRepository.SaveChanges();
            return list;
        }
    }
}
