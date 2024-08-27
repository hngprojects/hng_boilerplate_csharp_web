using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Organisations.Handlers
{
    public class DeleteUserOrganizationCommandHandler : IRequestHandler<DeleteUserOrganizationCommand, bool>
    {
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public DeleteUserOrganizationCommandHandler(IRepository<Organization> organizationRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteUserOrganizationCommand request, CancellationToken cancellationToken)
        {
            var org = await _organizationRepository.GetBySpec(x => x.Id == request.OrganizationId && x.OwnerId == request.UserId);
            if (org != null)
            {
                await _organizationRepository.DeleteAsync(org);
                await _organizationRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
