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
    public class GetProductsByNameQueryHandlerShould
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly IMapper _mapper;

        public GetProductsByNameQueryHandlerShould()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnAllProducts_WhenNameIsEmpty()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var handler = new GetProductsByNameQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByNameQuery("");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Product 1");
            Assert.Contains(result, p => p.Name == "Product 2");
        }

        [Fact]
        public async Task ReturnFilteredProducts_WhenNameIsProvided()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Apple" },
                new Product { Id = Guid.NewGuid(), Name = "Banana" },
                new Product { Id = Guid.NewGuid(), Name = "Orange" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var handler = new GetProductsByNameQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByNameQuery("an");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Banana");
            Assert.Contains(result, p => p.Name == "Orange");
            Assert.DoesNotContain(result, p => p.Name == "Apple");
        }

        [Fact]
        public async Task ReturnEmptyList_WhenNoProductsMatch()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Apple" },
                new Product { Id = Guid.NewGuid(), Name = "Banana" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var handler = new GetProductsByNameQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByNameQuery("Orange");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}