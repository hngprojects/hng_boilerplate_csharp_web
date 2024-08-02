using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Test.Features.Roles
{
    using AutoMapper;
    using Hng.Application.Features.Roles.Command;
    using Hng.Application.Features.Roles.Dto;
    using Hng.Application.Features.Roles.Handler;
    using Hng.Domain.Entities;
    using Hng.Infrastructure.Repository.Interface;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class UpdateRoleCommandHandlerTests
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateRoleCommandHandler _handler;

        public UpdateRoleCommandHandlerTests()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateRoleCommandHandler(_mockRoleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var requestDto = new UpdateRoleRequestDto { RoleId = Guid.NewGuid() };
            var command = new UpdateRoleCommand(requestDto);
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
            var requestDto = new UpdateRoleRequestDto { RoleId = Guid.NewGuid(), Name = "Updated Role", Description = "Updated description" };
            var command = new UpdateRoleCommand(requestDto);
            var existingRole = new Role { Id = requestDto.RoleId, Name = "Old Role", Description = "Old description" };
            _mockRoleRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existingRole);
            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateRoleCommand>(), It.IsAny<Role>())).Callback<UpdateRoleCommand, Role>((src, dest) =>
            {
                dest.Name = src.UPTRoleRequest.Name;
                dest.Description = src.UPTRoleRequest.Description;
            });
            _mockMapper.Setup(m => m.Map<UpdateRoleResponseDto>(It.IsAny<Role>())).Returns(new UpdateRoleResponseDto { StatusCode = 200, Message = "Role updated successfully" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Role updated successfully", result.Message);
        }

    }

}
