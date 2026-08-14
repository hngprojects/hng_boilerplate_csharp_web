using AutoMapper;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Handlers;
using Hng.Application.Features.NewsLetterSubscription.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.NewsLetterSubscription
{
    public class GetPaginatedSubscribersHandlerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly GetPaginatedSubscribersHandler _handler;

        public GetPaginatedSubscribersHandlerTests()
        {
            // Setup InMemory Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed data
            _context.NewsLetterSubscribers.AddRange(
                new() { Id = Guid.NewGuid(), Email = "user1@example.com", LeftOn = null, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Email = "user2@example.com", LeftOn = null, CreatedAt = DateTime.UtcNow }
            );
            _context.SaveChanges();

            // Mock AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Hng.Domain.Entities.NewsLetterSubscriber, NewsLetterSubscriptionDto>();
            });

            _mapper = config.CreateMapper();

            // Initialize handler
            _handler = new GetPaginatedSubscribersHandler(_context, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnActiveSubscribers_WithPagination()
        {
            // Arrange
            var query = new GetPaginatedSubscribersQuery { Page = 1, Limit = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Subscribers retrieved successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Pagination.Total); // Total should match seed data
        }

        [Fact]
        public async Task Handle_ShouldReturnNoSubscribers_WhenNoneExist()
        {
            // Arrange
            _context.NewsLetterSubscribers.RemoveRange(_context.NewsLetterSubscribers);
            await _context.SaveChangesAsync();

            var query = new GetPaginatedSubscribersQuery { Page = 1, Limit = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("No subscribers found", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenInvalidPaginationParams()
        {
            // Arrange
            var query = new GetPaginatedSubscribersQuery { Page = -1, Limit = 0 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid pagination parameters", result.Message);
        }
    }
}
