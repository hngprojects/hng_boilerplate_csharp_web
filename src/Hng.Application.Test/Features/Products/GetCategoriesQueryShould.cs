using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Products
{
    public class GetCategoriesQueryShould
    {
        private readonly Mock<IRepository<Category>> _mockRepository;
        private readonly IMapper _mapper;

        public GetCategoriesQueryShould()
        {
            _mockRepository = new Mock<IRepository<Category>>();

            // Set up AutoMapper with your profiles
            var config = new MapperConfiguration(cfg =>
            {
                // Add your AutoMapper profiles here
                cfg.CreateMap<Category, CategoryDto>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnCategoryDto_WhenCategoriesExists()
        {
            // Arrange
            var expectedCount = 3;
            var categories = new List<Category>
            {
                new Category{ Id = Guid.NewGuid(), Name = "Cloths", Description = "Cloths description here", Slug = "cloths", ParentId ="somerandomid"},
                new Category{ Id = Guid.NewGuid(), Name = "Electronics", Description = "Cloths description here", Slug = "electrical", ParentId ="somerandomid"},
                new Category{ Id = Guid.NewGuid(), Name = "Films", Description = "Cloths description here", Slug = "films", ParentId ="somerandomid"}
            };

            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            var productDto = new ProductDto { Id = productId, Name = "Test Product" };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var handler = new GetCategoriesQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetCategoriesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), expectedCount);
        }
    }
}
