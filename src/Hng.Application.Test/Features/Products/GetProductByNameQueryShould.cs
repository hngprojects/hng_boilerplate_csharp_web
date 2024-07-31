using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Test.Features.Products
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Moq;
    using Xunit;
    using System.Linq.Expressions;
    using global::Hng.Application.Features.Products.Dtos;
    using global::Hng.Application.Features.Products.Queries;
    using global::Hng.Domain.Entities;
    using global::Hng.Infrastructure.Repository.Interface;
    using global::Hng.Application.Features.Products.Handlers;

    namespace Hng.Application.Test.Features.Products
    {
        public class GetProductByNameQueryShould
        {
            private readonly Mock<IRepository<Product>> _mockRepository;
            private readonly IMapper _mapper;

            public GetProductByNameQueryShould()
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
                var productName = "Test Product";
                var product = new Product { Id = Guid.NewGuid(), Name = productName };
                var productDto = new ProductDto { Id = product.Id, Name = productName };

                _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                    .ReturnsAsync(product);

                var handler = new GetProductByNameQueryHandler(_mockRepository.Object, _mapper);
                var query = new GetProductByNameQuery(productName);

                // Act
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(productName, result.Name);
                Assert.Equal(product.Id, result.Id);
            }

            [Fact]
            public async Task ReturnNull_WhenProductNameIsEmpty()
            {
                // Arrange
                var handler = new GetProductByNameQueryHandler(_mockRepository.Object, _mapper);
                var query = new GetProductByNameQuery("");

                // Act
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task ReturnNull_WhenProductNotFound()
            {
                // Arrange
                var productName = "Non-existent Product";

                _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Product, bool>>>()))
                    .ReturnsAsync((Product)null);

                var handler = new GetProductByNameQueryHandler(_mockRepository.Object, _mapper);
                var query = new GetProductByNameQuery(productName);

                // Act
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
