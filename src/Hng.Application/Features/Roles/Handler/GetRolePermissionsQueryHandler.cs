using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Roles.Handler
{
    public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly IRepository<RolePermission> _permissionsRepo;
        private readonly IMapper _mapper;

        public GetRolePermissionsQueryHandler(IRepository<RolePermission> permissionsRepo, IMapper mapper)
        {
            _permissionsRepo = permissionsRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PermissionDto>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var rolePermissions = await _permissionsRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDto>>(rolePermissions);
        }
    }
}
