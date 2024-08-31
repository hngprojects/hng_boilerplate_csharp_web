using AutoMapper;
using Hng.Application.Features.Squeeze.Commands;
using Hng.Application.Features.Squeeze.Dtos;
using Hng.Application.Features.Squeeze.Handlers;
using Hng.Application.Features.Squeeze.Mappers;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Squeeze;

public class CreateSqueezeShould
{
    private readonly Mock<IRepository<Domain.Entities.Squeeze>> _repositoryMock;
    private readonly CreateSqueezeCommandHandler _handler;

    public CreateSqueezeShould()
    {
        var mappingProfile = new SqueezeMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        IMapper mapper = new Mapper(configuration);

        _repositoryMock = new Mock<IRepository<Domain.Entities.Squeeze>>();
        _handler = new CreateSqueezeCommandHandler(_repositoryMock.Object, mapper);
    }
    
    [Fact]
        public async Task Handle_ShouldReturnCreatedSqueeze()
        {
            var expectedId = Guid.NewGuid();
            var createDto = new CreateSqueezeRequestDto()
            {
                FirstName = "Test First Name",
                LastName = "Test Last Name",
                Email = "test@example.com",
            };

            var squeeze = new Domain.Entities.Squeeze
            {
                Id = expectedId,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Squeeze>()))
                .ReturnsAsync((Domain.Entities.Squeeze org) =>
                {
                    org.Id = expectedId;
                    return org;
                });

            var command = new CreateSqueezeCommand(createDto);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal("Squeeze Created Successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedId, result.Data.Id);
            Assert.Equal(createDto.FirstName, result.Data.FirstName);
            Assert.Equal(createDto.LastName, result.Data.LastName);
            Assert.Equal(createDto.Email, result.Data.Email);
        }
}