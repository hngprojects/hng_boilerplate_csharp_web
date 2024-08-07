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
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDetailsDto>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
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

            var response = _mapper.Map<RoleDetailsDto>(role);
            response.StatusCode = 200;
            response.Message = "Role details retrieved successfully";

            return response;
        }
    }

}
