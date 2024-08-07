namespace Hng.Application.Test.Features.Roles
{
    using AutoMapper;
    using Hng.Application.Features.Roles.Command;
    using Hng.Application.Features.Roles.Dto;
    using Hng.Application.Features.Roles.Handler;
    using Hng.Application.Features.Roles.Mappers;
    using Hng.Domain.Entities;
    using Hng.Infrastructure.Repository.Interface;
    using Moq;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class UpdateRoleCommandHandlerTests
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly IMapper _mapper;
        private readonly UpdateRoleCommandHandler _handler;

        public UpdateRoleCommandHandlerTests()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoleMappingProfile>();
            });
            _mapper = config.CreateMapper();
            _handler = new UpdateRoleCommandHandler(_mockRoleRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var requestDto = new UpdateRoleRequestDto { Name = "foo" };
            var command = new UpdateRoleCommand(Guid.NewGuid(), Guid.NewGuid(), requestDto);
            _mockRoleRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Role)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Role not found", result.Error);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var requestDto = new UpdateRoleRequestDto { Name = "Updated Role", Description = "Updated description" };
            var command = new UpdateRoleCommand(Guid.NewGuid(), roleId, requestDto);
            var existingRole = new Role { Id = roleId, Name = "Old Role", Description = "Old description" };
            _mockRoleRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(existingRole);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Role updated successfully", result.Message);
        }

    }

}
