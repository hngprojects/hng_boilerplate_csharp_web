using AutoMapper;
using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.NewsLetterSubscription
{
    public class AddSubscriberHandlerShould
    {
        private readonly Mock<IRepository<NewsLetterSubscriber>> _mockRepository;
        private readonly AddSubscriberHandler _handler;
        private readonly IMapper _mapper;

        public AddSubscriberHandlerShould()
        {
            _mockRepository = new Mock<IRepository<NewsLetterSubscriber>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsLetterSubscriptionDto, NewsLetterSubscriber>();
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new AddSubscriberHandler(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldAddSubscriber_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var email = "new_subscriber@gmail.com";
            var subscriber = new NewsLetterSubscriptionDto { Email = email };
            var command = new AddSubscriberCommand(subscriber);

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<NewsLetterSubscriber, bool>>>(), It.IsAny<System.Linq.Expressions.Expression<Func<NewsLetterSubscriber, object>>[]>()))
                .ReturnsAsync((NewsLetterSubscriber)null);

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<NewsLetterSubscriber>()))
                .ReturnsAsync((NewsLetterSubscriber)null);

            _mockRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<NewsLetterSubscriber>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenSubscriberAlreadyExists()
        {
            // Arrange
            var email = "existing_subscriber@gmail.com";
            var subscriberDto = new NewsLetterSubscriptionDto { Email = email };
            var command = new AddSubscriberCommand(subscriberDto);

            var existingSubscriber = new NewsLetterSubscriber { Email = email };

            _mockRepository.Setup(r => r.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<NewsLetterSubscriber, bool>>>(), It.IsAny<System.Linq.Expressions.Expression<Func<NewsLetterSubscriber, object>>[]>()))
                .ReturnsAsync(existingSubscriber);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<NewsLetterSubscriber>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }


    }
}
