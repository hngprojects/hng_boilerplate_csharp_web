using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;
using System.Linq.Expressions;


namespace Hng.Application.Test.Features.Products
{
    public class GetAllProductsShould
    {
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsShould()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockCategoryRepository = new Mock<IRepository<Category>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllProductsHandler(_mockProductRepository.Object, _mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProductsForOrganization()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllProductsQuery(orgId, null);

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Quantity = 5, OrganizationId = orgId },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Quantity = 0, OrganizationId = orgId }
            };

            var productDtos = new List<ProductResponseDto>
            {
                new ProductResponseDto { Id = products[0].Id, Name = "Product 1", Quantity = 5 },
                new ProductResponseDto { Id = products[1].Id, Name = "Product 2", Quantity = 0 }
            };

            _mockProductRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(productDtos);

            _mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var resultList = result.ToList();
            Assert.Equal("in stock", resultList[0].Status);
            Assert.Equal("out of stock", resultList[1].Status);

            _mockProductRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredProducts_WhenCategoryIsProvided()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var category = "Electronics";
            var query = new GetAllProductsQuery(orgId, category);

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Quantity = 5, OrganizationId = orgId, Category = "Electronics" },
            };

            var productDtos = new List<ProductResponseDto>
            {
                new ProductResponseDto { Id = products[0].Id, Name = "Product 1", Quantity = 5 }
            };

            var validCategories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = category } };

            _mockProductRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(productDtos);

            _mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(validCategories);


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one product matches category

            var resultList = result.ToList();
            Assert.Equal("in stock", resultList[0].Status);

            _mockProductRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyListWhenNoProducts()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllProductsQuery(orgId, null);

            _mockProductRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product>());

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(new List<ProductResponseDto>());

            _mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockProductRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldThrowError_WhenInvalidCategoryIsProvided()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var invalidCategory = "InvalidCategory";
            var query = new GetAllProductsQuery(orgId, invalidCategory);

            var validCategories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics" },
                new Category { Id = Guid.NewGuid(), Name = "Books" }
            };

            _mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(validCategories);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
               await _handler.Handle(query, CancellationToken.None));

            Assert.Equal($"Invalid category '{invalidCategory}' provided.", exception.Message);

            _mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
            _mockProductRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()), Times.Never);
        }
    }
}
