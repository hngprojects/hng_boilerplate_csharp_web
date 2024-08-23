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
    public class GetAllCategoriesQueryHandlerShould
    {
        private readonly Mock<IRepository<Category>> _categoryRepoMock;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandlerShould()
        {
            _categoryRepoMock = new Mock<IRepository<Category>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryMapperProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllCategories_ReturnsSuccessResponse()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category 1" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2" }
            };
            _categoryRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            var handler = new GetAllCategoriesQueryHandler(_categoryRepoMock.Object, _mapper);
            var query = new GetAllCategoriesQuery(new GetAllCategoriesQueryParams());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<PaginatedResponseDto<List<CategoryDto>>>(result);
            Assert.Equal(2, result.Data.Count);
        }
    }
}
