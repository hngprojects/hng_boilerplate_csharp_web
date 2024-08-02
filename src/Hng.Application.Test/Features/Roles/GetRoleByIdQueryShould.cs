using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Mappers;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class GetRoleByIdQueryHandlerTests
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly IMapper _mapper;
        private readonly GetRoleByIdQueryHandler _handler;

        public GetRoleByIdQueryHandlerTests()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoleMappingProfile>();
            });
            _mapper = config.CreateMapper();
            _handler = new GetRoleByIdQueryHandler(_mockRoleRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var query = new GetRoleByIdQuery(roleId, organizationId);
            _mockRoleRepository.Setup(repo => repo.GetBySpec(
                    It.IsAny<Expression<Func<Role, bool>>>(),
                    It.IsAny<Expression<Func<Role, object>>>()
                )).ReturnsAsync((Role)null);


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Not Found", result.Error);
        }


        [Fact]
        public async Task Handle_ValidRequest_ReturnsRoleDetails()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var query = new GetRoleByIdQuery(roleId, organizationId);
            var role = new Role { Id = roleId, OrganizationId = organizationId, Name = "Admin", Description = "Admin role" };

            _mockRoleRepository.Setup(repo => repo.GetBySpec(
                It.Is<Expression<Func<Role, bool>>>(expr => expr.Compile()(role)),
                It.IsAny<Expression<Func<Role, object>>>()
            )).ReturnsAsync(role);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Admin", result.Name);
            Assert.Equal("Admin role", result.Description);
        }



    }
}
