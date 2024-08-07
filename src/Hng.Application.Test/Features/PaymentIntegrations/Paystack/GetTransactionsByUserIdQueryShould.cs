using AutoMapper;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Handlers.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class GetTransactionsByUserIdQueryShould
    {
        private readonly Mock<IRepository<Transaction>> _mockRepository;
        private readonly IMapper _mapper;

        public GetTransactionsByUserIdQueryShould()
        {
            _mockRepository = new Mock<IRepository<Transaction>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnPagedListDto_WhenTransactionsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction { Id = Guid.NewGuid(), UserId = userId, Amount = 100 },
                new Transaction { Id = Guid.NewGuid(), UserId = userId, Amount = 200 }
            };

            _mockRepository.Setup(repo => repo.GetAllBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync(transactions);

            var handler = new GetTransactionsByUserIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetTransactionsByUserIdQuery(userId, 1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedListDto<TransactionDto>>(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.Equal(userId, item.UserId));
        }

        [Fact]
        public async Task ReturnEmptyPagedListDto_WhenNoTransactionsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetAllBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync(new List<Transaction>());

            var handler = new GetTransactionsByUserIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetTransactionsByUserIdQuery(userId, 1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedListDto<TransactionDto>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnCorrectPagination_WhenMultiplePages()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var transactions = Enumerable.Range(1, 25).Select(i => new Transaction { Id = Guid.NewGuid(), UserId = userId, Amount = i * 100 }).ToList();

            _mockRepository.Setup(repo => repo.GetAllBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync(transactions);

            var handler = new GetTransactionsByUserIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetTransactionsByUserIdQuery(userId, 2, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedListDto<TransactionDto>>(result);
            Assert.Equal(10, result.Count());
            Assert.Equal(2, result.MetaData.CurrentPage);
            Assert.Equal(3, result.MetaData.TotalPages);
            Assert.Equal(25, result.MetaData.TotalCount);
        }
    }
}