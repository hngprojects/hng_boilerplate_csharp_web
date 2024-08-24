using AutoMapper;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Features.ApiStatuses.Handlers.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.ApiStatuses
{
    public class GetAllStatusApiQueryShould
    {
        private readonly Mock<IRepository<ApiStatus>> _apistatusRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllApiStatusesHandler _handler;

        public GetAllStatusApiQueryShould()
        {
            _apistatusRepositoryMock = new Mock<IRepository<ApiStatus>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllApiStatusesHandler(_apistatusRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ReturnPaginatedResponseDto_WhenApiStatusesExist()
        {
            // Arrange
            var apiStatusList = new List<ApiStatus>
            {
                new ApiStatus
                {
                    Id = Guid.NewGuid(),
                    ApiGroup = "Group1",
                    Status = ApiStatusType.Operational,
                    ResponseTime = 120,
                    Details = "Service is running smoothly",
                    LastChecked = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },
                new ApiStatus
                {
                    Id = Guid.NewGuid(),
                    ApiGroup = "Group2",
                    Status = ApiStatusType.Down,
                    ResponseTime = 0,
                    Details = "Service is currently down",
                    LastChecked = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            };

            var apiStatusDtoList = new List<ApiStatusResponseDto>
            {
                new ApiStatusResponseDto
                {

                    ApiGroup = "Group1",
                    Status = ApiStatusType.Operational,
                    ResponseTime = 120,
                    Details = "Service is running smoothly",
                    LastChecked = apiStatusList[0].LastChecked,

                },
                new ApiStatusResponseDto
                {
                    ApiGroup = "Group2",
                    Status = ApiStatusType.Down,
                    ResponseTime = 0,
                    Details = "Service is currently down",
                    LastChecked = apiStatusList[1].LastChecked,
                }
            };

            _apistatusRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(apiStatusList);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ApiStatusResponseDto>>(apiStatusList))
                .Returns(apiStatusDtoList);

            var request = new GetAllApiStatusesQueries { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PaginatedResponseDto<List<ApiStatusResponseDto>>>(result);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(apiStatusDtoList.Count, result.Data.Count);
        }

        [Fact]
        public async Task ReturnEmptyPaginatedResponseDto_WhenNoApiStatusesExist()
        {
            // Arrange
            var emptyApiStatusList = new List<ApiStatus>();

            _apistatusRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(emptyApiStatusList);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ApiStatusResponseDto>>(emptyApiStatusList))
                .Returns(new List<ApiStatusResponseDto>());

            var request = new GetAllApiStatusesQueries { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PaginatedResponseDto<List<ApiStatusResponseDto>>>(result);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _apistatusRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            var request = new GetAllApiStatusesQueries { PageNumber = 1, PageSize = 10 };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}