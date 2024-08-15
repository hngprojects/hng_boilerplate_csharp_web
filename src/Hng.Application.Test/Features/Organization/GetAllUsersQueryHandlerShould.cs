using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Application.Features.Organisations.Queries;
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
                Users = new List<User>
        {
            new User {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            },
            new User {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "0987654321"
            }
        }
            };

            var expectedDto = new OrganizationUserDto
            {
                Users = organization.Users
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
            Assert.Equal(expectedDto.Users.Count, result.Users.Count);
            Assert.All(result.Users, user =>
            {
                Assert.Contains(organization.Users, orgUser => orgUser.Id == user.Id);
            });

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