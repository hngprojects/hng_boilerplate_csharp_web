using AutoMapper;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Handler;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class CreateRoleCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateRoleCommandHandler _handler;

        public CreateRoleCommandShould()
        {
            _mockOrganizationRepository = new Mock<IRepository<Domain.Entities.Organization>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateRoleCommandHandler(_mockOrganizationRepository.Object, _mockRoleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_OrganizationNotFound_ReturnsNotFound()
        {
            // Arrange
            var roleRequestDto = new CreateRoleRequestDto
            {
                Name = "RoleName",
                Description = "RoleDescription"
            };
            var command = new CreateRoleCommand(Guid.NewGuid(), roleRequestDto);

            _mockOrganizationRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Organisation not found", result.Error);
        }

        [Fact]
        public async Task Handle_RoleAlreadyExists_ReturnsConflict()
        {
            // Arrange
            var roleRequestDto = new CreateRoleRequestDto
            {
                Name = "Admin",
                Description = "RoleDescription"
            };
            var command = new CreateRoleCommand(Guid.NewGuid(), roleRequestDto);

            _mockOrganizationRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new Domain.Entities.Organization());
            _mockRoleRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new Role());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Conflict", result.Error);
        }


        [Fact]
        public async Task Handle_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var roleRequestDto = new CreateRoleRequestDto
            {
                Name = "Admin",
                Description = "Admin role"
            };

            var command = new CreateRoleCommand(Guid.NewGuid(), roleRequestDto);

            // Set up the mock for organization repository to return a valid organization
            _mockOrganizationRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new Domain.Entities.Organization());

            // Set up the mock for role repository to simulate no existing role found
            _mockRoleRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync((Role)null);

            // Set up the mock for AutoMapper
            var role = new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Admin role" };
            _mockMapper.Setup(m => m.Map<Role>(It.IsAny<CreateRoleCommand>())).Returns(role);
            _mockMapper.Setup(m => m.Map<CreateRoleResponseDto>(It.IsAny<Role>())).Returns(new CreateRoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                StatusCode = 201,
                Message = "Role created successfully"
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Role created successfully", result.Message);
            Assert.Equal(role.Name, result.Name);
            Assert.Equal(role.Description, result.Description);
        }

    }
}
