using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Timezones.Handlers.Commands
{
    public class UpdateTimezoneCommandHandler : IRequestHandler<UpdateTimezoneCommand, TimezoneResponseDto>
    {
        private readonly IRepository<Timezone> _repository;
        private readonly IMapper _mapper;

        public UpdateTimezoneCommandHandler(IRepository<Timezone> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimezoneResponseDto> Handle(UpdateTimezoneCommand request, CancellationToken cancellationToken)
        {
            var existingTimezone = await _repository.GetAsync(request.Id);
            if (existingTimezone == null)
            {
                return new TimezoneResponseDto
                {
                    StatusCode = 404,
                    Message = "Timezone not found"
                };
            }

            _mapper.Map(request, existingTimezone);
            await _repository.UpdateAsync(existingTimezone);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<TimezoneDto>(existingTimezone);
            return new TimezoneResponseDto
            {
                Timezone = responseDto,
                StatusCode = 200,
                Message = "Timezone updated successfully"
            };
        }
    }
}