using AutoMapper;
using Hng.Application.Features.Languages.Handlers.Queries;
using Hng.Application.Features.Languages.Mappers;
using Hng.Application.Features.Languages.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Languages
{
    public class GetLanguageByIdQueryShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Language>> _repositoryMock;
        private readonly GetLanguageByIdQueryHandler _handler;

        public GetLanguageByIdQueryShould()
        {
            var mappingProfile = new LanguageMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);
            _repositoryMock = new Mock<IRepository<Language>>();
            _handler = new GetLanguageByIdQueryHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnLanguage_WhenLanguageExists()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var query = new GetLanguageByIdQuery
            {
                Id = expectedId
            };
            var existingLanguage = new Language
            {
                Id = expectedId,
                Name = "English"
            };
            _repositoryMock.Setup(r => r.GetAsync(expectedId))
                .ReturnsAsync(existingLanguage);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Language retrieved successfully", result.Message);
            Assert.Equal(expectedId, result.Data.Id);
            Assert.Equal("English", result.Data.Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenLanguageDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var query = new GetLanguageByIdQuery
            {
                Id = nonExistentId
            };
            _repositoryMock.Setup(r => r.GetAsync(nonExistentId))
                .ReturnsAsync((Language)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Language not found", result.Message);
            Assert.Null(result.Data);
        }
    }
}