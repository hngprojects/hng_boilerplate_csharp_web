using AutoMapper;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Roles.Handler
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponseDto>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<UpdateRoleResponseDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId && r.OrganizationId == request.OrgId);

            if (role is null)
            {
                return new UpdateRoleResponseDto
                {
                    StatusCode = 404,
                    Error = "Role not found"
                };
            }


            _mapper.Map(request, role);

            await _roleRepository.UpdateAsync(role);

            var response = _mapper.Map<UpdateRoleResponseDto>(role);
            response.StatusCode = 200;
            response.Message = "Role updated successfully";

            return response;
        }
    }
}
