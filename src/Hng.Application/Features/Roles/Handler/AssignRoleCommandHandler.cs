using Hng.Application.Features.Roles.Command;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using JasperFx.Core;
using MediatR;

namespace Hng.Application.Features.Roles.Handler
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, SuccessResponseDto<object>>
    {
        private readonly IRepository<Organization> _organisationRepository;
        private readonly IRepository<Role> _roleRepository;

        public AssignRoleCommandHandler(IRepository<Organization> organisationRepository, IRepository<Role> roleRepository)
        {
            _organisationRepository = organisationRepository;
            _roleRepository = roleRepository;
        }
        public async Task<SuccessResponseDto<object>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            var org = await _organisationRepository.GetBySpec(o => o.Id == request.OrgId && o.Users.Any(u => u.Id == request.UserId), o => o.UsersRoles);
            var role = await _roleRepository.GetBySpec(r => r.Id == request.RoleId);

            if (org is null || role is null)
                return null;
            org.UsersRoles.Where(ur => ur.UserId == request.UserId).First().Role = role;
            await _organisationRepository.UpdateAsync(org);
            await _organisationRepository.SaveChanges();

            return new SuccessResponseDto<object> { Message = "Assign Role", Data = { } };
        }
    }
}
