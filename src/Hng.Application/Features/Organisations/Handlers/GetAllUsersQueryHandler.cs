using AutoMapper;
using Hng.Application.Features.Organisations.Queries;
using Hng.Infrastructure.Repository.Interface;
using Hng.Domain.Entities;
using MediatR;
using Hng.Application.Features.Organisations.Dtos;

namespace Hng.Application.Features.Organisations.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OrganizationUserDto>
    {
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IRepository<Organization> organizationRepository, IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        public async Task<OrganizationUserDto> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {

            var org = await _organizationRepository.GetBySpec(x => x.Id == request.OrganizationId, u => u.Users);
            if (org is null)
            {
                return null;
            }
            return _mapper.Map<OrganizationUserDto>(org);
        }
    }
}
