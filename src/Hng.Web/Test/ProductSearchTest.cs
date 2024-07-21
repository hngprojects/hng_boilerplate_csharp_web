namespace Hng.Web.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hng.Application.Services;
    using Hng.Domain.Entities;
    using Hng.Infrastructure.Context;
    using Hng.Infrastructure.Models;
    using Hng.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    namespace ProductSearchApi.Tests
    {
        public class ProductServiceTests
        {
            private readonly Mock<MyDBContext> _mockContext;
            private readonly Mock<DbSet<Product>> _mockSet;
            private readonly List<Product> _products;

            public ProductServiceTests()
            {
                _products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Category = "Category A", Price = 190 },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Category = "Category B", Price = 20 },
                new Product { Id = Guid.NewGuid(), Name = "Another Product", Category = "Category A", Price = 30 },
                new Product { Id = Guid.NewGuid(), Name = "Different Item", Category = "Category C", Price = 40 },
            };

                _mockSet = new Mock<DbSet<Product>>();
                _mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(_products.AsQueryable().Provider);
                _mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(_products.AsQueryable().Expression);
                _mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(_products.AsQueryable().ElementType);
                _mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(_products.GetEnumerator());

                _mockContext = new Mock<MyDBContext>();
                _mockContext.Setup(c => c.Products).Returns(_mockSet.Object);
            }

            [Fact]
            public void SearchProducts_ReturnsAllProducts_WhenNoParametersProvided()
            {
                // Arrange
                var service = new ProductService(_mockContext.Object);
                var parameters = new SearchParameters();

                // Act
                var result = service.SearchProducts(parameters, 1, 10);

                // Assert
                Assert.Equal(4, result.TotalItems);
                Assert.Equal(4, result.Items.Count);
            }

            [Fact]
            public void SearchProducts_FiltersByName_WhenNameParameterProvided()
            {
                // Arrange
                var service = new ProductService(_mockContext.Object);
                var parameters = new SearchParameters { Name = "Product" };

                // Act
                var result = service.SearchProducts(parameters, 1, 10);

                // Assert
                Assert.Equal(3, result.TotalItems);
                Assert.All(result.Items, item => Assert.Contains("Product", item.Name));
            }

            [Fact]
            public void SearchProducts_FiltersByCategory_WhenCategoryParameterProvided()
            {
                // Arrange
                var service = new ProductService(_mockContext.Object);
                var parameters = new SearchParameters { Category = "Category A" };

                // Act
                var result = service.SearchProducts(parameters, 1, 10);

                // Assert
                Assert.Equal(2, result.TotalItems);
                Assert.All(result.Items, item => Assert.Equal("Category A", item.Category));
            }

            [Fact]
            public void SearchProducts_FiltersByPriceRange_WhenPriceParametersProvided()
            {
                // Arrange
                var service = new ProductService(_mockContext.Object);
                var parameters = new SearchParameters { MinPrice = 20, MaxPrice = 35 };

                // Act
                var result = service.SearchProducts(parameters, 1, 10);

                // Assert
                Assert.Equal(2, result.TotalItems);
                Assert.All(result.Items, item => Assert.InRange(item.Price, 20, 35));
            }

            [Fact]
            public void SearchProducts_PaginatesResults_WhenPageParametersProvided()
            {
                // Arrange
                var service = new ProductService(_mockContext.Object);
                var parameters = new SearchParameters();

                // Act
                var result = service.SearchProducts(parameters, 2, 2);

                // Assert
                Assert.Equal(4, result.TotalItems);
                Assert.Equal(2, result.Items.Count);
                Assert.Equal(2, result.Page);
                Assert.Equal(2, result.PageSize);
                Assert.Equal(2, result.TotalPages);
            }
        }
    }
}
