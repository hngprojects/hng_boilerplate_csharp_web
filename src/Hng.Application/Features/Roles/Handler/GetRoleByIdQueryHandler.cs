using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Application.Shared.Dtos;
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
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDetailsResponseDto>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDetailsResponseDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId && r.OrganizationId == request.OrganizationId, r => r.Permissions);
            if (role == null)
            {
                return new RoleDetailsResponseDto
                {
                    StatusCode = 404,
                    Error = "Not Found",
                    Message = $"The role with ID {request.RoleId} does not exist"
                };
            }

            var details = _mapper.Map<RoleDetails>(role);
            var response = new RoleDetailsResponseDto
            {
                StatusCode = 200,
                Message = "Role details retrieved successfully",
                Data = details
            };

            return response;
        }
    }

}
