using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Organisations.Handlers
{
    public class GetUsersOrganizationsHandler : IRequestHandler<GetAllUsersOrganizationsQuery, SuccessResponseDto<List<OrganizationDto>>>
    {
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetUsersOrganizationsHandler(IRepository<Organization> organizationRepository, IRepository<User> userRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<SuccessResponseDto<List<OrganizationDto>>> Handle(GetAllUsersOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var orgs = await _organizationRepository.GetAllBySpec(x => x.OwnerId == userId);
            var orgsList = _mapper.Map<List<OrganizationDto>>(orgs);
            return new SuccessResponseDto<List<OrganizationDto>>
            {
                Data = orgsList,
                Message = "Success"
            };
        }
    }
}
