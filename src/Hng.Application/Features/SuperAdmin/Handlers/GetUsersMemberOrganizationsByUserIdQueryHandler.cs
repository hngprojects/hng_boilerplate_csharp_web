using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Handlers;

public class GetUsersMemberOrganizationsByUserIdQueryHandler :IRequestHandler<GetUsersMemberOrganizationsByUserIdQuery, PagedListDto<OrganizationDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    

    public GetUsersMemberOrganizationsByUserIdQueryHandler(IRepository<User> userRepository,
        IRepository<Organization> organizationRepository, IMapper mapper, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }


    public async Task<PagedListDto<OrganizationDto>> Handle(GetUsersMemberOrganizationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user =await _userRepository.GetBySpec(user => user.Id == request.UserId, user=>user.Organizations);
        
        if (user==null)
        {
            return null;
        }
        var userMemberOrganizations = user.Organizations.Where(org=>org.OwnerId!=user.Id).ToList();
        
        
        var mappedOrganizations = _mapper.Map<List<OrganizationDto>>(userMemberOrganizations);
            
        // var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);

        var organizationResult = PagedListDto<OrganizationDto>.ToPagedList(mappedOrganizations,
            request.UserMemberOrganizationsQueryParameter.Offset, request.UserMemberOrganizationsQueryParameter.Limit);
        return organizationResult ;
    }
}