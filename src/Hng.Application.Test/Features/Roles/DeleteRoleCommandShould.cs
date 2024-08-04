using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Handler;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class DeleteRoleCommandShould
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly DeleteRoleCommandHandler _handler;

        public DeleteRoleCommandShould()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _handler = new DeleteRoleCommandHandler(_mockRoleRepository.Object);
        }
        [Fact]
        public async Task Handle_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var command = new DeleteRoleCommand(Guid.NewGuid(), Guid.NewGuid());
            _mockRoleRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync((Role)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Not Found", result.Error);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var command = new DeleteRoleCommand(Guid.NewGuid(), Guid.NewGuid());
            var role = new Role { Id = command.RoleId, OrganizationId = command.OrganizationId };
            _mockRoleRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(role);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Role successfully removed", result.Message);
        }

    }
}
