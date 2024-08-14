using AutoMapper;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Roles.Handler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponseDto>
    {
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RolePermission> _permissionsRepository;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IRepository<Organization> organizationRepository, IRepository<Role> roleRepository, IRepository<RolePermission> permissionsRepository, IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _roleRepository = roleRepository;
            _permissionsRepository = permissionsRepository;
            _mapper = mapper;
        }

        public async Task<CreateRoleResponseDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(request.OrganizationId);
            if (organization == null)
            {
                return new CreateRoleResponseDto
                {
                    StatusCode = 404,
                    Error = "Organisation not found",
                    Message = $"The organisation with ID {request.OrganizationId} does not exist",
                    Data = null
                };
            }

            var existingRole = await _roleRepository.GetBySpec(r => r.Name == request.RoleRequestBody.Name && r.OrganizationId == request.OrganizationId);
            if (existingRole != null)
            {
                return new CreateRoleResponseDto
                {
                    StatusCode = 409,
                    Error = "Conflict",
                    Message = "A role with this name already exists in the organization",
                    Data = null
                };
            }

            var permissions = await _permissionsRepository.GetAllBySpec(p => request.RoleRequestBody.Permissions.Contains(p.Id));
            var role = _mapper.Map<Role>(request);
            role.Id = Guid.NewGuid();
            role.OrganizationId = request.OrganizationId;
            role.IsActive = true;
            role.CreatedAt = DateTime.UtcNow;

            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChanges();
            role.Permissions = (ICollection<RolePermission>)permissions;
            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChanges();

            var response = _mapper.Map<CreateRoleResponseDto>(role);
            response.StatusCode = 201;
            response.Message = "Role created successfully";

            return response;
        }
    }

}
