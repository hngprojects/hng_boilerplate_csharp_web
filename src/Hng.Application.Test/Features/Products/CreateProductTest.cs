﻿using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Handlers;
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
        private readonly Mock<IRepository<Product>> _mock;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateProductHandler _handler;

        public CreateProductTest()
        {
            _mock = new Mock<IRepository<Product>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateProductHandler(_mock.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsProductDto()
        {
            var productCreationDto = new ProductCreationDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m
            };

            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m
            };

            var command = new CreateProductCommand(productCreationDto);

            _mockMapper.Setup(m => m.Map<Product>(productCreationDto)).Returns(product);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);
            _mock.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<ProductDto>(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Description, result.Description);
            Assert.Equal(productDto.Price, result.Price);

            _mock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Product>(It.IsAny<ProductCreationDto>()), Times.Once);
            _mockMapper.Verify(m => m.Map<ProductDto>(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            var productCreationDto = new ProductCreationDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m
            };

            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m
            };

            var command = new CreateProductCommand(productCreationDto);

            _mockMapper.Setup(m => m.Map<Product>(productCreationDto)).Returns(product);
            _mock.Setup(r => r.AddAsync(It.IsAny<Product>())).ThrowsAsync(new System.Exception("Database error"));

            await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Product>(It.IsAny<ProductCreationDto>()), Times.Once);
            _mockMapper.Verify(m => m.Map<ProductDto>(It.IsAny<Product>()), Times.Never);
        }
    }
}
