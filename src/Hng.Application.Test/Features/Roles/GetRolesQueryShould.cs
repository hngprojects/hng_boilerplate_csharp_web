using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Roles
{
    public class GetRolesQueryShould
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetRolesQueryHandler _handler;

        public GetRolesQueryShould()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetRolesQueryHandler(_mockRoleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_NoRoles_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetRolesQuery(Guid.NewGuid());
            _mockRoleRepository.Setup(repo => repo.GetAllBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsRoles()
        {
            // Arrange
            var query = new GetRolesQuery(Guid.NewGuid());
            var roles = new List<Role>
            {
                new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Admin role" },
                new Role { Id = Guid.NewGuid(), Name = "User", Description = "User role" }
            };
            _mockRoleRepository.Setup(repo => repo.GetAllBySpec(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(roles);
            _mockMapper.Setup(m => m.Map<IEnumerable<RoleDto>>(It.IsAny<IEnumerable<Role>>())).Returns(new List<RoleDto>
            {
                new RoleDto { Id = roles[0].Id, Name = roles[0].Name, Description = roles[0].Description },
                new RoleDto { Id = roles[1].Id, Name = roles[1].Name, Description = roles[1].Description }
            });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Admin");
            Assert.Contains(result, r => r.Name == "User");
        }

    }
}
