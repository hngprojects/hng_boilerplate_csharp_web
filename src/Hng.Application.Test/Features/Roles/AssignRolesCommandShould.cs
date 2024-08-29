using AutoMapper;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.UserManagement.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class AssignRolesCommandShould
    {
        private IMapper _mapper;
        private Mock<IRepository<Domain.Entities.Organization>> _orgRepo;
        private Mock<IRepository<Role>> _roleRepo;

        public AssignRolesCommandShould()
        {
            var cfg = new MapperConfiguration(opt =>
            {
                opt.AddProfile<UserMappingProfile>();
            });
            _mapper = cfg.CreateMapper();
            _orgRepo = new Mock<IRepository<Hng.Domain.Entities.Organization>>();
            _roleRepo = new Mock<IRepository<Role>>();

        }

        [Fact]
        public async Task Handler_Should_Assign_User_Role_In_Organization()
        {
            var roleId = Guid.NewGuid();
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var org = new Hng.Domain.Entities.Organization()
            {
                Id = orgId,
                Name = "Test Org",
                Email = "Test@email.com",
                Users = [
                    new()
                    {
                        Id = userId,
                        FirstName = "test"
                    }
                    ],
                UsersRoles = [
                    new()
                    {
                        RoleId = roleId,
                        UserId = userId,
                        OrganizationId = orgId,
                    }
                    ]
            };

            _orgRepo.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Hng.Domain.Entities.Organization, bool>>>(), It.IsAny<Expression<Func<Hng.Domain.Entities.Organization, object>>[]>()))
                .ReturnsAsync(org);

            _roleRepo.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(new Role
                {
                    Id = userId,

                });

            var handler = new AssignRoleCommandHandler(_orgRepo.Object, _roleRepo.Object);
            var result = await handler.Handle(new AssignRoleCommand(orgId, roleId, userId), CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Assign Role", result.Message);
        }
    }
}
