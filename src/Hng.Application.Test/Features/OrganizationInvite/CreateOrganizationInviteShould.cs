// using Xunit;
// using Moq;
// using AutoMapper;
// using Hng.Application.Features.OrganisationInvite.Commands;
// using Hng.Application.Features.OrganisationInvite.Handlers;
// using Hng.Application.Features.OrganisationInvite.Dtos;
// using Hng.Domain.Entities;
// using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;
// using Hng.Infrastructure.Services.Interfaces;
// using Hng.Application.Features.OrganisationInvite.Mappers;
// using Hng.Infrastructure.Utilities;
// using Microsoft.AspNetCore.Http;

// namespace Hng.Application.Tests.Features.OrganisationInvite.Handlers
// {
//     public class CreateOrganizationInviteCommandHandlerTests
//     {
//         private readonly Mock<IOrganisationInviteService> _mockService;
//         private readonly IMapper _mapper;

//         private readonly CreateOrganizationInviteCommandHandler _handler;

//         public CreateOrganizationInviteCommandHandlerTests()
//         {
//             _mockService = new Mock<IOrganisationInviteService>();
//             OrganizationInviteMapperProfile mapperProfile = new();
//             MapperConfiguration config = new(cfg => cfg.AddProfile(mapperProfile));
//             _mapper = new Mapper(config);
//             _handler = new CreateOrganizationInviteCommandHandler(_mockService.Object, _mapper);
//         }

//         [Fact]
//         public async Task Handle_WithValidRequest_ShouldReturnSuccessResult()
//         {
//             // Arrange
//             var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
//             {
//                 OrganizationId = Guid.NewGuid().ToString(),
//                 UserId = Guid.NewGuid(),
//                 Email = "test@example.com"
//             });

//             var organizationInvite = new OrganizationInvite()
//             {
//             };

//             _mockService.Setup(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
//                 .ReturnsAsync(Result<OrganizationInvite>.Success(organizationInvite));

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);
//             // Assert
//             Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
//             Assert.NotNull(result.Data);

//         }

//         [Fact]
//         public async Task Handle_WithInviteAlreadyExistsError_ShouldReturnConflictResponse()
//         {
//             // Arrange
//             var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
//             {
//                 OrganizationId = "invalid-guid",
//                 UserId = Guid.NewGuid(),
//                 Email = "test@example.com"
//             });

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
//         }

//         [Fact]
//         public async Task Handle_ForExistingInvite_ShouldReturnFailureResult()
//         {
//             // Arrange
//             var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
//             {
//                 OrganizationId = Guid.NewGuid().ToString(),
//                 UserId = Guid.NewGuid(),
//                 Email = "test@example.com"
//             });

//             var expectedError = InviteAlreadyExistsError.FromEmail(command.InviteDto.Email);

//             _mockService.Setup(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
//                 .ReturnsAsync(Result<OrganizationInvite>.Failure(expectedError));

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);

//             Assert.Equal(expectedError.Message, result.Message);
//             // Assert
//             Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
//         }
//         [Fact]
//         public async Task Handle_WithOrganisationDoesNotExistError_ShouldReturnNotFoundResponse()
//         {
//             Guid orgId = Guid.NewGuid();
//             // Arrange
//             var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
//             {
//                 OrganizationId = orgId.ToString(),
//                 UserId = Guid.NewGuid(),
//                 Email = "test@example.com"
//             });

//             var expectedError = OrganisationDoesNotExistError.FromId(orgId);
//             _mockService.Setup(x => x.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
//                 .ReturnsAsync(Result<OrganizationInvite>.Failure(expectedError));

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
//             Assert.Equal(expectedError.Message, result.Message);
//         }

//         [Fact]
//         public async Task Handle_WithUserIsNotOwnerError_ShouldReturnUnauthorizedResponse()
//         {

//             Guid orgId = Guid.NewGuid();
//             Guid userId = Guid.NewGuid();

//             // Arrange
//             var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
//             {
//                 OrganizationId = orgId.ToString(),
//                 UserId = orgId,
//                 Email = "test@example.com"
//             });

//             var expectedError = UserIsNotOwnerError.FromIds(userId, orgId);

//             _mockService.Setup(x => x.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
//                 .ReturnsAsync(Result<OrganizationInvite>.Failure(expectedError));

//             // Act
//             var result = await _handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
//             Assert.Equal(expectedError.Message, result.Message);
//         }
//     }
// }