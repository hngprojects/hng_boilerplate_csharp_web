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
using System.Linq.Expressions;

namespace Hng.Application.Test.Features.Products
{
    public class CreateProductHandlerShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerShould()
        {
            var mappingProfile = new ProductMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockOrganizationRepository = new Mock<IRepository<Domain.Entities.Organization>>();
            _mockAuthService = new Mock<IAuthenticationService>();

            _handler = new CreateProductHandler(
                _mockProductRepository.Object,
                _mockOrganizationRepository.Object,
                _mockAuthService.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateProductSuccessfully()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var productCreationDto = new ProductCreationDto
            {
                Name = "Test Product",
                Price = 100.0m,
                Description = "Test Description",
                Category = "Test Category",
                Quantity = 10
            };

            var command = new CreateProductCommand(orgId, productCreationDto);

            var organization = new Domain.Entities.Organization { Id = orgId };

            _mockAuthService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockOrganizationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync(organization);

            _mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            _mockProductRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productCreationDto.Name, result.Name);
            Assert.Equal(productCreationDto.Price, result.Price);
            Assert.Equal(productCreationDto.Description, result.Description);
            Assert.Equal(productCreationDto.Quantity, result.Quantity); // Added assertion for quantity

            _mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockProductRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenOrganizationNotFound()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var productCreationDto = new ProductCreationDto
            {
                Name = "Test Product",
                Price = 100.0m,
                Description = "Test Description",
                Category = "Test Category",
                Quantity = 10
            };

            var command = new CreateProductCommand(orgId, productCreationDto);

            _mockOrganizationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
            _mockProductRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}