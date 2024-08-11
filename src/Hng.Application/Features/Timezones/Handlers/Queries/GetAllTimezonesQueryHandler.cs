using AutoMapper;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Application.Features.Timezones.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Timezones.Handlers.Queries
{
    public class GetAllTimezonesQueryHandler : IRequestHandler<GetAllTimezonesQuery, PaginatedResponseDto<List<TimezoneDto>>>
    {
        private readonly IRepository<Timezone> _timezoneRepository;
        private readonly IMapper _mapper;

        public GetAllTimezonesQueryHandler(IRepository<Timezone> timezoneRepository, IMapper mapper)
        {
            _timezoneRepository = timezoneRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponseDto<List<TimezoneDto>>> Handle(GetAllTimezonesQuery request, CancellationToken cancellationToken)
        {
            var timezones = await _timezoneRepository.GetAllAsync();
            var timezoneDtos = _mapper.Map<IEnumerable<TimezoneDto>>(timezones);
            var paginatedTimezones = PagedListDto<TimezoneDto>.ToPagedList(
                timezoneDtos,
                request.PageNumber,
                request.PageSize
            );

            return new PaginatedResponseDto<List<TimezoneDto>>
            {
                Data = paginatedTimezones.ToList(),
                Metadata = paginatedTimezones.MetaData
            };
        }
    }
}