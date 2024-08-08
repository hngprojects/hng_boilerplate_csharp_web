using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Products
{
    public class AddProductsTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Product>> _repositoryMock;
        private readonly AddProductsHandler _handler;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public AddProductsTest()
        {
            var mappingProfile = new ProductMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _repositoryMock = new Mock<IRepository<Product>>();
            _handler = new AddProductsHandler(_repositoryMock.Object, _mapper, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAddedListOfProducts()
        {
            var expectedId = Guid.NewGuid();

            var addProductsDto = new List<ProductCreationDto>()
            {
                new ProductCreationDto
                {
                    Name = "shoe",
                    Description = "Testing shoe",
                    Category = "Footwear",
                    Price = 3000,
                },
                new ProductCreationDto
                {
                    Name = "shoe",
                    Description = "Testing shoe",
                    Category = "Footwear",
                    Price = 3000,
                }
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product org) =>
                {
                    org.Id = expectedId;
                    return org;
                });

            var command = new AddProductsCommand(addProductsDto);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(addProductsDto.Count, result.Products.Count);
            Assert.Equal(addProductsDto[1].Name, result.Products[1].Name);
            Assert.Equal(addProductsDto[1].Description, result.Products[1].Description);
            Assert.Equal(addProductsDto[1].Category, result.Products[1].Category);
            Assert.Equal(addProductsDto[1].Price, result.Products[1].Price);
            Assert.Equal(addProductsDto[2].Name, result.Products[2].Name);
            Assert.Equal(addProductsDto[2].Description, result.Products[2].Description);
            Assert.Equal(addProductsDto[2].Category, result.Products[2].Category);
            Assert.Equal(addProductsDto[2].Price, result.Products[2].Price);
        }
    }
}
