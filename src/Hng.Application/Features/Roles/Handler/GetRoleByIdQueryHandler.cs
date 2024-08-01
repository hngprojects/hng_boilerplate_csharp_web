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
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDetailsDto>
    {
        private readonly IRepository<Role> _roleRepository;

        public GetRoleByIdQueryHandler(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDetailsDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId && r.OrganizationId == request.OrganizationId, r => r.Permissions);
            if (role == null)
            {
                return new RoleDetailsDto
                {
                    StatusCode = 404,
                    Error = "Not Found",
                    Message = $"The role with ID {request.RoleId} does not exist"
                };
            }

            return new RoleDetailsDto
            {
                StatusCode = 200,
                Id = role.Id.ToString(),
                Name = role.Name,
                Description = role.Description,
                Permissions = role.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name=p.Name,
                    Description = p.Description,
                }).ToList()
            };
        }
    }

}
