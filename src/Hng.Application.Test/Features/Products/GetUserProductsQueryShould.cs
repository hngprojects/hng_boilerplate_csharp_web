using AutoMapper;
using FluentAssertions;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Products
{
    public class GetUserProductsQueryShould
    {
        private readonly Mock<IRepository<Domain.Entities.Product>> _productRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly GetUserProductsQueryHandler _handler;

        public GetUserProductsQueryShould()
        {
            _productRepositoryMock = new Mock<IRepository<Domain.Entities.Product>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.Product, ProductDto>();
                cfg.CreateMap<User, User>();
            });
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetUserProductsQueryHandler(_productRepositoryMock.Object, _mapper, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ProductsExist_ReturnsListOfProductDto()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var products = new List<Domain.Entities.Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Product 1",
                    Description = "Testing Product 1",
                    Category = "Footwear",
                    Price = 3000
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Product 2",
                    Description = "Testing Product 2",
                    Category = "Footwear",
                    Price = 3000
                }
            };

            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _productRepositoryMock.Setup(repo => repo.GetAllBySpec(x => x.UserId == userId))
                .ReturnsAsync(products);
            var query = new GetUserProductsQuery(new GetProductsQueryParameters { PageNumber = 1, PageSize = 2 });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Test Product 1");
            result.Last().Name.Should().Be("Test Product 2");
        }

        [Fact]
        public async Task Handle_NoProductsExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _productRepositoryMock.Setup(repo => repo.GetAllBySpec(x => x.UserId == userId)).ReturnsAsync(new List<Domain.Entities.Product>());

            var query = new GetUserProductsQuery(new GetProductsQueryParameters { PageNumber = 1, PageSize = 2 });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
