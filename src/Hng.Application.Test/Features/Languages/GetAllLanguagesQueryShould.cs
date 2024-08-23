using AutoMapper;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Handlers.Queries;
using Hng.Application.Features.Languages.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Languages
{
    public class GetAllLanguagesQueryShould
    {
        private readonly Mock<IRepository<Language>> _languageRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetAllLanguagesQueryHandler _handler;

        public GetAllLanguagesQueryShould()
        {
            _languageRepositoryMock = new Mock<IRepository<Language>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Language, LanguageDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetAllLanguagesQueryHandler(_languageRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsPaginatedResponse_WhenLanguagesExist()
        {
            // Arrange
            var languages = new List<Language>
            {
                new Language { Id = Guid.NewGuid(), Name = "English" },
                new Language { Id = Guid.NewGuid(), Name = "Spanish" }
            };
            _languageRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(languages);
            var query = new GetAllLanguagesQuery { Offset = 1, Limit = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(1, result.Metadata.CurrentPage);
            Assert.Equal(2, result.Metadata.TotalCount);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyPaginatedResponse_WhenNoLanguagesExist()
        {
            // Arrange
            _languageRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Language>());
            var query = new GetAllLanguagesQuery { Offset = 1, Limit = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Metadata.TotalCount);
        }
    }
}