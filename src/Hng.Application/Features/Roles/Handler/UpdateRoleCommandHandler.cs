using AutoMapper;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
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
            var role = await _roleRepository.GetAsync(request.UPTRoleRequest.RoleId);

            if (role == null)
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
