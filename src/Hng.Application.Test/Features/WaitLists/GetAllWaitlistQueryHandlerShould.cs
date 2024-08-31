using AutoMapper;
using Hng.Application.Features.WaitLists.Handlers;
using Hng.Application.Features.WaitLists.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.WaitLists
{
    public class GetAllWaitlistQueryHandlerShould
    {
        private readonly Mock<IRepository<Waitlist>> _mockWaitlistRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllWaitlistQueryHandler _handler;

        public GetAllWaitlistQueryHandlerShould()
        {
            _mockWaitlistRepository = new Mock<IRepository<Waitlist>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllWaitlistQueryHandler(
                _mockWaitlistRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnAllWaitlists()
        {
            // Arrange
            var waitlists = new List<Waitlist>
            {
                new Waitlist { Id = Guid.NewGuid(), Email = "test1@example.com" },
                new Waitlist { Id = Guid.NewGuid(), Email = "test2@example.com" }
            };

            _mockWaitlistRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(waitlists);

            var query = new GetAllWaitlistQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(waitlists, result);

            _mockWaitlistRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyListWhenNoWaitlists()
        {
            // Arrange
            var emptyList = new List<Waitlist>();

            _mockWaitlistRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyList);

            var query = new GetAllWaitlistQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockWaitlistRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}