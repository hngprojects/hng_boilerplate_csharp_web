﻿using AutoMapper;
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
    public class GetRolesQueryShould
    {
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly IMapper _mapper;
        private readonly GetRolesQueryHandler _handler;

        public GetRolesQueryShould()
        {
            _mockRoleRepository = new Mock<IRepository<Role>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoleMappingProfile>();
            });
            _mapper = config.CreateMapper();
            _handler = new GetRolesQueryHandler(_mockRoleRepository.Object, _mapper);
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
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Admin");
            Assert.Contains(result, r => r.Name == "User");
        }

    }
}
