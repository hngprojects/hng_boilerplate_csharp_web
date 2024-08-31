using AutoMapper;
using Hng.Application.Features.WaitLists.Commands;
using Hng.Application.Features.WaitLists.Dtos;
using Hng.Application.Features.WaitLists.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.WaitLists
{
    public class CreateWaitlistCommandHandlerShould
    {
        private readonly Mock<IRepository<Waitlist>> _mockWaitlistRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateWaitlistCommandHandler _handler;

        public CreateWaitlistCommandHandlerShould()
        {
            _mockWaitlistRepository = new Mock<IRepository<Waitlist>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateWaitlistCommandHandler(
                _mockWaitlistRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateNewWaitlistWhenEmailDoesNotExist()
        {
            // Arrange
            var waitListDto = new WaitListDto
            {
                Name = "John Doe",
                Email = "new@example.com"
            };

            var command = new CreateWaitlistCommand(waitListDto);
            var newWaitlist = new Waitlist
            {
                Id = Guid.NewGuid(),
                Name = waitListDto.Name,
                Email = waitListDto.Email
            };

            _mockWaitlistRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Waitlist, bool>>>()))
                .ReturnsAsync((Waitlist)null);
            _mockMapper.Setup(m => m.Map<Waitlist>(waitListDto))
                .Returns(newWaitlist);
            _mockWaitlistRepository.Setup(r => r.AddAsync(It.IsAny<Waitlist>()))
              .ReturnsAsync(newWaitlist);
            _mockWaitlistRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(waitListDto.Name, result.Name);
            Assert.Equal(waitListDto.Email, result.Email);

            _mockWaitlistRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<Waitlist, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Waitlist>(waitListDto), Times.Once);
            _mockWaitlistRepository.Verify(r => r.AddAsync(It.IsAny<Waitlist>()), Times.Once);
            _mockWaitlistRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNullWhenEmailAlreadyExists()
        {
            // Arrange
            var existingEmail = "existing@example.com";
            var waitListDto = new WaitListDto
            {
                Name = "John Doe",
                Email = existingEmail
            };
            var command = new CreateWaitlistCommand(waitListDto);
            var existingWaitlist = new Waitlist
            {
                Id = Guid.NewGuid(),
                Email = existingEmail
            };

            _mockWaitlistRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Waitlist, bool>>>()))
                .ReturnsAsync(existingWaitlist);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockWaitlistRepository.Verify(r => r.GetBySpec(It.IsAny<Expression<Func<Waitlist, bool>>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Waitlist>(It.IsAny<WaitListDto>()), Times.Never);
            _mockWaitlistRepository.Verify(r => r.AddAsync(It.IsAny<Waitlist>()), Times.Never);
            _mockWaitlistRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
