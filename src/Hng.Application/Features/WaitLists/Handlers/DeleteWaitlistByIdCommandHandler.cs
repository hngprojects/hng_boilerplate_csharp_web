using AutoMapper;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Hng.Domain.Entities;
using Hng.Application.Features.WaitLists.Commands;

namespace Hng.Application.Features.WaitLists.Handlers
{
    public class DeleteWaitlistByIdCommandHandler : IRequestHandler<DeleteWaitlistByIdCommand, Waitlist>
    {
        private readonly IRepository<Waitlist> _waitlistRepository;
        private readonly IMapper _mapper;
        public DeleteWaitlistByIdCommandHandler(IRepository<Waitlist> waitlistRepository, IMapper mapper)
        {
            _waitlistRepository = waitlistRepository;
            _mapper = mapper;
        }
        public async Task<Waitlist> Handle(DeleteWaitlistByIdCommand request, CancellationToken cancellationToken)
        {
            var list = await _waitlistRepository.GetAsync(request.WaitListId);

            if (list != null)
            {
                await _waitlistRepository.DeleteAsync(list);
                await _waitlistRepository.SaveChanges();
                return list;
            }

            return null;
        }
    }
}
