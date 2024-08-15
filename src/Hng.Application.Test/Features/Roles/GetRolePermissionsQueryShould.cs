using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Mappers;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class GetRolePermissionsQueryShould
    {
        private readonly GetRolePermissionsQueryHandler _getRolePermissionsQueryHandler;
        private readonly Mock<IRepository<RolePermission>> _permissionsRepoMock;
        private readonly IMapper _mapper;
        public GetRolePermissionsQueryShould()
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<RoleMappingProfile>();
            });
            _permissionsRepoMock = new Mock<IRepository<RolePermission>>();
            _mapper = config.CreateMapper();
            _getRolePermissionsQueryHandler = new GetRolePermissionsQueryHandler(_permissionsRepoMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_Success_Response()
        {
            var query = new GetRolePermissionsQuery();
            List<RolePermission> permissions = new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "Can Create Users"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "Can Edit Users",
                    Description = "Allows Edit another Users Profile"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "Can Blacklist/Whitelist Users",
                    Description = "Allows ban or unban a user"
                }
            };
            _permissionsRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(permissions);

            var result = await _getRolePermissionsQueryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(typeof(List<PermissionDto>), result.GetType());
        }
    }
}
