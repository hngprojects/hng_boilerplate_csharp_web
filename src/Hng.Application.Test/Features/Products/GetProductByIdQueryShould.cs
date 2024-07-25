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
        private readonly IMapper _mapper;

        public GetProductByIdQueryShould()
        {
            _mockRepository = new Mock<IRepository<Product>>();

            // Set up AutoMapper with your profiles
            var config = new MapperConfiguration(cfg =>
            {
                // Add your AutoMapper profiles here
                cfg.CreateMap<Product, ProductDto>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnProductDto_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            var productDto = new ProductDto { Id = productId, Name = "Test Product" };

            _mockRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(product);

            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
        }


        [Fact]
        public async Task ReturnNull_WhenProductIdIsEmpty()
        {
            // Arrange
            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByIdQuery(Guid.Empty);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task ReturnNull_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync((Product)null);

            var handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

    }
}
