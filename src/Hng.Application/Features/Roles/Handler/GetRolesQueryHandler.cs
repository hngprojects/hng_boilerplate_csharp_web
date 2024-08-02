using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Handler
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllBySpec(r => r.OrganizationId == request.OrganizationId);
            if (roles is null)
            {
                return Enumerable.Empty<RoleDto>();
            }
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }
    }

}
