using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class GetProductByIdQueryShould
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetProductByIdQueryShould()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ReturnProductDto_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            var productDto = new ProductDto { Id = productId, Name = "Test Product" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductDto>(product))
                .Returns(productDto);

            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task ThrowArgumentException_WhenProductIdIsEmpty()
        {
            // Arrange
            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
            var query = new GetProductByIdQuery(Guid.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowKeyNotFoundException_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
            var query = new GetProductByIdQuery(productId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
