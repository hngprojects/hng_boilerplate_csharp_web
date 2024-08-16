using AutoMapper;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Handlers.Queries;
using Hng.Application.Features.Categories.Mappers;
using Hng.Application.Features.Categories.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Categories
{
    public class GetCategoryByIdQueryHandlerShould
    {
        private readonly Mock<IRepository<Category>> _categoryRepoMock;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandlerShould()
        {
            _categoryRepoMock = new Mock<IRepository<Category>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryMapperProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetCategoryById_ExistingCategory_ReturnsSuccessResponse()
        {
            // Arrange
            var existingCategory = new Category { Id = Guid.NewGuid(), Name = "Existing Category" };
            _categoryRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCategory);

            var handler = new GetCategoryByIdQueryHandler(_categoryRepoMock.Object, _mapper);
            var query = new GetCategoryByIdQuery(existingCategory.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<CategoryDto>>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Existing Category", result.Data.Name);
        }

        [Fact]
        public async Task GetCategoryById_NonExistingCategory_ReturnsNotFoundResponse()
        {
            // Arrange
            _categoryRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category)null);

            var handler = new GetCategoryByIdQueryHandler(_categoryRepoMock.Object, _mapper);
            var query = new GetCategoryByIdQuery(Guid.NewGuid());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<CategoryDto>>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Null(result.Data);
        }
    }
}
