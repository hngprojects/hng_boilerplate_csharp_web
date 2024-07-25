using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Mappers;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Product
{
    public class UpdateProductShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductShould()
        {
            var mappingProfile = new ProductMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _repositoryMock = new Mock<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedProduct()
        {
            var productId = Guid.NewGuid();
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product Name",
                Price = 600.0m,
                Description = "Updated Product Description",
                Category = "New Category"
            };

            var initialUpdatedAt = DateTime.UtcNow.AddDays(-1); 
            var updatedAt = DateTime.UtcNow;
            var existingProduct = new Domain.Entities.Product
            {
                Id = productId,
                Name = "Old Product Name",
                Price = 500.0m,
                Description = "Old Product Description",
                Category = "Old Category",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = initialUpdatedAt
            };

            var updatedProduct = new Domain.Entities.Product
            {
                Id = productId,
                Name = updateDto.Name,
                Price = updateDto.Price,
                Description = updateDto.Description,
                Category = updateDto.Category,
                CreatedAt = existingProduct.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            var updatedProductDto = new ProductDto
            {
                Id = productId,
                Name = updateDto.Name,
                Price = updateDto.Price,
                Description = updateDto.Description,
                Category = updateDto.Category,
                CreatedAt = existingProduct.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _repositoryMock.Setup(r => r.Update(It.IsAny<Domain.Entities.Product>()))
                .Callback<Domain.Entities.Product>(product =>
                {
                    product.Id = productId;
                    product.Name = updateDto.Name;
                    product.Price = updateDto.Price;
                    product.Description = updateDto.Description;
                    product.Category = updateDto.Category;
                    product.UpdatedAt = updatedAt;
                });

            _repositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new UpdateProductCommand(productId, updateDto);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.Price, result.Price);
            Assert.Equal(updateDto.Description, result.Description);
            Assert.Equal(updateDto.Category, result.Category);
            Assert.Equal(existingProduct.CreatedAt, result.CreatedAt);
            Assert.True(result.UpdatedAt > initialUpdatedAt);
        }
    }
}
