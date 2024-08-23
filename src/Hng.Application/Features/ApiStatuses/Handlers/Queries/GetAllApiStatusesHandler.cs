using AutoMapper;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Application.Features.Timezones.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.ApiStatuses.Handlers.Queries
{
    public class GetAllApiStatusesHandler : IRequestHandler<GetAllApiStatusesQueries, PaginatedResponseDto<List<ApiStatusResponseDto>>>
    {
        private readonly IRepository<ApiStatus> _apistatusRepository;
        private readonly IMapper _mapper;

        public GetAllApiStatusesHandler(IRepository<ApiStatus> apistatusRepository, IMapper mapper)
        {
            _apistatusRepository = apistatusRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponseDto<List<ApiStatusResponseDto>>> Handle(GetAllApiStatusesQueries request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            var pageSize = request.PageSize > 0 ? request.PageSize : 10;
            var apiStatus = await _apistatusRepository.GetAllAsync();
            var apiStatusDtos = _mapper.Map<List<ApiStatusResponseDto>>(apiStatus);
            var paginatedApiStatus = PagedListDto<ApiStatusResponseDto>.ToPagedList(
                apiStatusDtos,
                pageNumber,
                pageSize
            );

            return new PaginatedResponseDto<List<ApiStatusResponseDto>>
            {
                Data = paginatedApiStatus.ToList(),
                Metadata = paginatedApiStatus.MetaData
            };
        }
    }
}
