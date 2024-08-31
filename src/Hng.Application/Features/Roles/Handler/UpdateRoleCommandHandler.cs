using AutoMapper;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Roles.Handler
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, CreateRoleResponseDto>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RolePermission> _permissionsRepository;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRepository<Role> roleRepository, IRepository<RolePermission> permissionsRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _permissionsRepository = permissionsRepository;
            _mapper = mapper;
        }

        public async Task<CreateRoleResponseDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId && r.OrganizationId == request.OrgId);

            if (role is null)
            {
                return new CreateRoleResponseDto
                {
                    StatusCode = 404,
                    Error = "Role not found"
                };
            }


            _mapper.Map(request, role);
            var permissions = await _permissionsRepository.GetAllBySpec(p => request.UPTRoleRequest.Permissions.Contains(p.Id));

            role.Permissions = (ICollection<RolePermission>)permissions;

            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChanges();

            var response = _mapper.Map<CreateRoleResponseDto>(role);
            response.StatusCode = 200;
            response.Message = "Role updated successfully";

            return response;
        }
    }
}
