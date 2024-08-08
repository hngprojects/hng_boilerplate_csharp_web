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
			var userId = Guid.NewGuid();
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

			_authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
			_repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
				.ReturnsAsync((Product product) =>
				{
					product.Id = expectedId;
					return product;
				});

			var command = new AddProductsCommand(addProductsDto);

			var result = await _handler.Handle(command, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(addProductsDto[0].Name, result.Products[0].Name);
			Assert.Equal(addProductsDto[0].Description, result.Products[0].Description);
			Assert.Equal(addProductsDto[0].Category, result.Products[0].Category);
			Assert.Equal(addProductsDto[0].Price, result.Products[0].Price);
			Assert.Equal(addProductsDto.Count, result.Products.Count);
			Assert.Equal(addProductsDto[1].Name, result.Products[1].Name);
			Assert.Equal(addProductsDto[1].Description, result.Products[1].Description);
			Assert.Equal(addProductsDto[1].Category, result.Products[1].Category);
			Assert.Equal(addProductsDto[1].Price, result.Products[1].Price);

		}
	}
}
