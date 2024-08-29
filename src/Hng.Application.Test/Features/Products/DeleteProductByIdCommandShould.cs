using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Products
{
    public class DeleteProductByIdCommandShould
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly IMapper _mapper;
        List<Product> products;

        public DeleteProductByIdCommandShould()
        {
            _mockRepository = new Mock<IRepository<Product>>();

            // Set up AutoMapper with your profiles
            var config = new MapperConfiguration(cfg =>
            {
                // Add your AutoMapper profiles here
                cfg.CreateMap<Product, ProductDto>();
            });

            _mapper = config.CreateMapper();

            // setup test data
            products = new List<Product>
            {
                new Product{ Id = new Guid("9BF7A95A-40F5-463F-9CEC-3C492A1810BC"), Name = "Fine Cloths", Description = "a fine cloth", Category ="Shirt", Price = new decimal(10.00), UserId = Guid.NewGuid()},
                new Product{ Id = Guid.NewGuid(), Name = "60 Electrical bulb", Description = "a bulb", Category ="Film", Price = new decimal(1.35), UserId = Guid.NewGuid()},
                new Product{ Id = Guid.NewGuid(), Name = "First Blood", Description = "what a film", Category ="Film", Price = new decimal(45.56), UserId = Guid.NewGuid()}
            };
        }

        [Fact]
        public async Task DeleteProduct_WhenProductExists()
        {
            // Arrange
            var productId = new Guid("9BF7A95A-40F5-463F-9CEC-3C492A1810BC");

            _mockRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(products.FirstOrDefault(v => v.Id == productId));

            var handler = new DeleteProductByIdCommandHandler(_mockRepository.Object, _mapper);
            var query = new DeleteProductByIdCommand(productId);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Id, productId);
        }


        [Fact]
        public async Task DoesNotDelete_WhenProductIdNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var handler = new DeleteProductByIdCommandHandler(_mockRepository.Object, _mapper);
            var query = new DeleteProductByIdCommand(productId);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(actual);
        }
    }
}
