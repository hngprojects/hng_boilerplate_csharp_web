using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Mappers;
using Xunit;

namespace Hng.Application.Test.Features.Organization
{
    public class GetOrganizationByIdQueryShould
    {
        private readonly IMapper _mapper;

        public GetOrganizationByIdQueryShould()
        {
            var organizationMappingProfile = new OrganizationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(organizationMappingProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public void Map_Organization_To_OrganizationDto()
        {
            // Arrange
            var expected = new Domain.Entities.Organization
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "Test Organization",
                Description = "Description",
                Slug = "test-org",
                Industry = "Technology",
                Type = "Startup",
                Country = "Country",
                Address = "123 Test St",
                State = "Test State",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = Guid.NewGuid()
            };

            // Act
            var result = _mapper.Map<OrganizationDto>(expected);

            // Assert
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Email, result.Email);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Description, result.Description);
            Assert.Equal(expected.Slug, result.Slug);
            Assert.Equal(expected.Industry, result.Industry);
            Assert.Equal(expected.Type, result.Type);
            Assert.Equal(expected.Country, result.Country);
            Assert.Equal(expected.Address, result.Address);
            Assert.Equal(expected.State, result.State);
            Assert.Equal(expected.CreatedAt, result.CreatedAt);
            Assert.Equal(expected.UpdatedAt, result.UpdatedAt);
            Assert.Equal(expected.OwnerId, result.OwnerId);
        }
    }
}
