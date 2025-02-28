using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Handlers
{
    public class GetUsersOwnedOrganizationsByUserIdQueryHandler : IRequestHandler<GetUsersOwnedOrganizationsByUserIdQuery, PagedListDto<OrganizationDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetUsersOwnedOrganizationsByUserIdQueryHandler(IRepository<User> userRepository,
            IRepository<Organization> organizationRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<OrganizationDto>> Handle(GetUsersOwnedOrganizationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetBySpec(user => user.Id == request.UserId, user => user.Organizations);

            if (user == null)
            {
                return null;
            }

            var userOwnedOrganizations = user.Organizations.Where(org => org.OwnerId == user.Id).ToList();

            var mappedOrganizations = _mapper.Map<List<OrganizationDto>>(userOwnedOrganizations);

            var organizationResult = PagedListDto<OrganizationDto>.ToPagedList(mappedOrganizations, request.UserOwnedOrganizationsQueryParameter.Offset, request.UserOwnedOrganizationsQueryParameter.Limit);

            return organizationResult;
        }
    }
}
