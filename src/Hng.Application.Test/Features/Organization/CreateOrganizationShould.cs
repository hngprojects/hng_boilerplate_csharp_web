using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Application.Features.Organisations.Mappers;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Organization
{
    public class CreateOrganizationShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _repositoryMock;
        private readonly CreateOrganisationCommandHandler _handler;

        public CreateOrganizationShould()
        {
            var mappingProfile = new OrganizationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            IMapper mapper = new Mapper(configuration);

            _repositoryMock = new Mock<IRepository<Domain.Entities.Organization>>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new CreateOrganisationCommandHandler(_repositoryMock.Object, mapper, authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedOrganization()
        {
            var expectedId = Guid.NewGuid();
            var createDto = new CreateOrganizationDto
            {
                Name = "Test Org",
                Description = "A test organization",
                Email = "test@example.com",
                Industry = "Tech",
                Type = "Startup",
                Country = "Country",
                Address = "Address",
                State = "State"
            };

            var organization = new Domain.Entities.Organization
            {
                Id = expectedId,
                Name = createDto.Name,
                Description = createDto.Description,
                Email = createDto.Email,
                Industry = createDto.Industry,
                Type = createDto.Type,
                Country = createDto.Country,
                Address = createDto.Address,
                State = createDto.State,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Organization>()))
                .ReturnsAsync((Domain.Entities.Organization org) =>
                {
                    org.Id = expectedId;
                    return org;
                });

            var command = new CreateOrganizationCommand(createDto);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal("Organisation Created Successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedId, result.Data.Id);
            Assert.Equal(createDto.Name, result.Data.Name);
            Assert.Equal(createDto.Description, result.Data.Description);
            Assert.Equal(createDto.Email, result.Data.Email);
            Assert.Equal(createDto.Industry, result.Data.Industry);
            Assert.Equal(createDto.Type, result.Data.Type);
            Assert.Equal(createDto.Country, result.Data.Country);
            Assert.Equal(createDto.Address, result.Data.Address);
            Assert.Equal(createDto.State, result.Data.State);
        }
    }
}
