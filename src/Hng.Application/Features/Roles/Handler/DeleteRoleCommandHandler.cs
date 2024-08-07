using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Handler
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResponseDto>
    {
        private readonly IRepository<Role> _roleRepository;

        public DeleteRoleCommandHandler(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<DeleteRoleResponseDto> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId && r.OrganizationId == request.OrganizationId);
            if (role == null)
            {
                return new DeleteRoleResponseDto
                {
                    StatusCode = 404,
                    Error = "Not Found",
                    Message = $"The role with ID {request.RoleId} does not exist"
                };
            }

            role.IsActive = false;
            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChanges();

            return new DeleteRoleResponseDto
            {
                StatusCode = 200,
                Message = "Role successfully removed"
            };
        }
    }

}
