using AutoMapper;
using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Faqs
{
    public class UpdateFaqCommandShould
    {
        private readonly Mock<IRepository<Faq>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateFaqCommandHandler _handler;

        public UpdateFaqCommandShould()
        {
            _repositoryMock = new Mock<IRepository<Faq>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<FaqMappingProfile>());
            _mapper = config.CreateMapper();

            _handler = new UpdateFaqCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateFaq_WhenFaqExists()
        {
            // Arrange
            var faqId = Guid.NewGuid();
            var faqRequest = new UpdateFaqRequestDto { Question = "Updated Question", Answer = "Updated Answer" };
            var faq = new Faq { Id = faqId, Question = "Old Question", Answer = "Old Answer" };

            _repositoryMock.Setup(x => x.GetAsync(faqId)).ReturnsAsync(faq);

            var command = new UpdateFaqCommand(faqId, faqRequest);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(x => x.UpdateAsync(faq), Times.Once);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("FAQ updated successfully", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenFaqDoesNotExist()
        {
            // Arrange
            var faqId = Guid.NewGuid();
            var faqRequest = new UpdateFaqRequestDto { Question = "Updated Question", Answer = "Updated Answer" };

            _repositoryMock.Setup(x => x.GetAsync(faqId)).ReturnsAsync((Faq)null);

            var command = new UpdateFaqCommand(faqId, faqRequest);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("FAQ not found", result.Message);
        }
    }

}
