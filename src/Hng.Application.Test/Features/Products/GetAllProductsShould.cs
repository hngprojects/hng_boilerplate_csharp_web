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
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsShould()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllProductsHandler(_mockProductRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProductsForOrganization()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllProductsQuery(orgId);

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
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyListWhenNoProducts()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var query = new GetAllProductsQuery(orgId);

            _mockProductRepository.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product>());

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(new List<ProductResponseDto>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockProductRepository.Verify(r => r.GetAllBySpec(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()), Times.Once);
        }
    }
}