using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Products
{
    public class CreateProductTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Product>> _repositoryMock;
        private readonly CreateProductHandler _handler;

        public CreateProductTest()
        {
            var mappingProfile = new ProductMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _repositoryMock = new Mock<IRepository<Product>>();
            _handler = new CreateProductHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedProduct()
        {
            var expectedId = Guid.NewGuid();
            var createDto = new ProductCreationDto
            {
                Name = "shoe",
                Description = "Testing shoe",
                Category = "Footwear",
                Price = 3000,
            };

            var production = new Product
            {
                Id = expectedId,
                Name = createDto.Name,
                Description = createDto.Description,
                Category = createDto.Category,
                Price = createDto.Price,
                UserId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product org) =>
                {
                    org.Id = expectedId;
                    return org;
                });

            var command = new CreateProductCommand(production.UserId.ToString(),createDto);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Description, result.Description);
            Assert.Equal(createDto.Category, result.Category);
            Assert.Equal(createDto.Price, result.Price);
        }
    }
}
