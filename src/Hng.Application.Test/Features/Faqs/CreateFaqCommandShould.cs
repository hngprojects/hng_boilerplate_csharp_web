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
    public class CreateFaqCommandShould
    {
        private readonly Mock<IRepository<Faq>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateFaqCommandHandler _handler;

        public CreateFaqCommandShould()
        {
            _repositoryMock = new Mock<IRepository<Faq>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<FaqMappingProfile>());
            _mapper = config.CreateMapper();

            _handler = new CreateFaqCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateFaq_WhenRequestIsValid()
        {
            // Arrange
            var faqRequest = new CreateFaqRequestDto { Question = "Test Question", Answer = "Test Answer" };
            var command = new CreateFaqCommand(faqRequest);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Faq>()), Times.Once);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("FAQ created successfully", result.Message);
        }
    }

}
