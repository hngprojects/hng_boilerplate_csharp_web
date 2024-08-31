using AutoMapper;
using Hng.Application.Features.WaitLists.Commands;
using Hng.Application.Features.WaitLists.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.WaitLists
{
    public class DeleteWaitlistByIdCommandHandlerShould
    {
        private readonly Mock<IRepository<Waitlist>> _mockWaitlistRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DeleteWaitlistByIdCommandHandler _handler;

        public DeleteWaitlistByIdCommandHandlerShould()
        {
            _mockWaitlistRepository = new Mock<IRepository<Waitlist>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeleteWaitlistByIdCommandHandler(
                _mockWaitlistRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteWaitlistWhenExists()
        {
            // Arrange
            var waitlistId = Guid.NewGuid();
            var existingWaitlist = new Waitlist { Id = waitlistId, Email = "test@example.com" };

            _mockWaitlistRepository.Setup(r => r.GetAsync(waitlistId))
                .ReturnsAsync(existingWaitlist);
            _mockWaitlistRepository.Setup(r => r.DeleteAsync(existingWaitlist))
                .ReturnsAsync(existingWaitlist);
            _mockWaitlistRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new DeleteWaitlistByIdCommand(waitlistId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingWaitlist, result);

            _mockWaitlistRepository.Verify(r => r.GetAsync(waitlistId), Times.Once);
            _mockWaitlistRepository.Verify(r => r.DeleteAsync(existingWaitlist), Times.Once);
            _mockWaitlistRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenWaitlistDoesNotExist()
        {
            // Arrange
            var nonExistentWaitlistId = Guid.NewGuid();

            _mockWaitlistRepository.Setup(r => r.GetAsync(nonExistentWaitlistId))
                .ReturnsAsync((Waitlist)null);

            var command = new DeleteWaitlistByIdCommand(nonExistentWaitlistId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockWaitlistRepository.Verify(r => r.GetAsync(nonExistentWaitlistId), Times.Once);
            _mockWaitlistRepository.Verify(r => r.DeleteAsync(It.IsAny<Waitlist>()), Times.Never);
            _mockWaitlistRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}