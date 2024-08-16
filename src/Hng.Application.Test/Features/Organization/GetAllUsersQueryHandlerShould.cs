using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Organisations
{
    public class GetAllUsersQueryHandlerShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerShould()
        {
            _mockOrganizationRepository = new Mock<IRepository<Domain.Entities.Organization>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllUsersQueryHandler(_mockOrganizationRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOrganizationUserDto_WhenOrganizationExists()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllUsersQuery(orgId);

            var organization = new Domain.Entities.Organization
            {
                Id = orgId,
                Name = "Test Org",
                Description = "Test Description",
                Slug = "test-org",
                Email = "test@org.com",
                Industry = "Technology",
                Type = "Corporation",
                Country = "USA",
                Address = "123 Test St",
                State = "CA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = Guid.NewGuid(),
                IsActive = true,
                Users = new List<User>
                {
                    new User {
                        Id = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        PhoneNumber = "1234567890",
                        AvatarUrl = "https://example.com/avatar1.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsSuperAdmin = false
                    },
                    new User {
                        Id = Guid.NewGuid(),
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "jane.smith@example.com",
                        PhoneNumber = "0987654321",
                        AvatarUrl = "https://example.com/avatar2.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsSuperAdmin = true
                    }
                }
            };

            var expectedDto = new OrganizationUserDto
            {
                Name = organization.Name,
                Description = organization.Description,
                Slug = organization.Slug,
                Email = organization.Email,
                Industry = organization.Industry,
                Type = organization.Type,
                Country = organization.Country,
                Address = organization.Address,
                State = organization.State,
                CreatedAt = organization.CreatedAt,
                UpdatedAt = organization.UpdatedAt,
                OwnerId = organization.OwnerId,
                IsActive = organization.IsActive,
                Users = organization.Users.Select(u => new UserOrganzationDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AvatarUrl = u.AvatarUrl,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    IsSuperAdmin = u.IsSuperAdmin
                }).ToList()
            };

            _mockOrganizationRepository.Setup(r => r.GetBySpec(
                It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Organization, object>>>()))
                .ReturnsAsync(organization);

            _mockMapper.Setup(m => m.Map<OrganizationUserDto>(It.IsAny<Domain.Entities.Organization>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.Description, result.Description);
            Assert.Equal(expectedDto.Slug, result.Slug);
            Assert.Equal(expectedDto.Email, result.Email);
            Assert.Equal(expectedDto.Industry, result.Industry);
            Assert.Equal(expectedDto.Type, result.Type);
            Assert.Equal(expectedDto.Country, result.Country);
            Assert.Equal(expectedDto.Address, result.Address);
            Assert.Equal(expectedDto.State, result.State);
            Assert.Equal(expectedDto.CreatedAt, result.CreatedAt);
            Assert.Equal(expectedDto.UpdatedAt, result.UpdatedAt);
            Assert.Equal(expectedDto.OwnerId, result.OwnerId);
            Assert.Equal(expectedDto.IsActive, result.IsActive);

            Assert.NotNull(result.Users);
            Assert.NotNull(expectedDto.Users);
            Assert.Equal(expectedDto.Users.Count, result.Users.Count);

            var expectedUsersList = expectedDto.Users.ToList();
            var actualUsersList = result.Users.ToList();

            for (int i = 0; i < actualUsersList.Count; i++)
            {
                var expectedUser = expectedUsersList[i];
                var actualUser = actualUsersList[i];

                Assert.NotNull(expectedUser);
                Assert.NotNull(actualUser);

                Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
                Assert.Equal(expectedUser.LastName, actualUser.LastName);
                Assert.Equal(expectedUser.Email, actualUser.Email);
                Assert.Equal(expectedUser.PhoneNumber, actualUser.PhoneNumber);
                Assert.Equal(expectedUser.AvatarUrl, actualUser.AvatarUrl);
                Assert.Equal(expectedUser.CreatedAt, actualUser.CreatedAt);
                Assert.Equal(expectedUser.UpdatedAt, actualUser.UpdatedAt);
                Assert.Equal(expectedUser.IsSuperAdmin, actualUser.IsSuperAdmin);
            }

            _mockOrganizationRepository.Verify(r => r.GetBySpec(
                It.Is<Expression<Func<Domain.Entities.Organization, bool>>>(expr => expr.Compile()(organization)),
                It.IsAny<Expression<Func<Domain.Entities.Organization, object>>>()), Times.Once);

            _mockMapper.Verify(m => m.Map<OrganizationUserDto>(It.Is<Domain.Entities.Organization>(o => o.Id == orgId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenOrganizationDoesNotExist()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllUsersQuery(orgId);

            _mockOrganizationRepository.Setup(r => r.GetBySpec(
                It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Organization, object>>>()))
                .ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockOrganizationRepository.Verify(r => r.GetBySpec(
                It.Is<Expression<Func<Domain.Entities.Organization, bool>>>(expr => expr.Compile()(new Domain.Entities.Organization { Id = orgId })),
                It.IsAny<Expression<Func<Domain.Entities.Organization, object>>>()), Times.Once);

            _mockMapper.Verify(m => m.Map<OrganizationUserDto>(It.IsAny<Domain.Entities.Organization>()), Times.Never);
        }
    }
}