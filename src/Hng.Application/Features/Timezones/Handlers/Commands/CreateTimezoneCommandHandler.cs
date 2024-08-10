using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Timezones.Handlers.Commands
{
    public class CreateTimezoneCommandHandler : IRequestHandler<CreateTimezoneCommand, TimezoneResponseDto>
    {
        private readonly IRepository<Timezone> _repository;
        private readonly IMapper _mapper;

        public CreateTimezoneCommandHandler(IRepository<Timezone> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimezoneResponseDto> Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
        {
            var existingTimezone = await _repository.GetBySpec(t => t.TimezoneValue == request.Timezone);
            if (existingTimezone != null)
            {
                return new TimezoneResponseDto
                {
                    StatusCode = 409,
                    Message = "Timezone already exists"
                };
            }

            var timezone = _mapper.Map<Timezone>(request);
            timezone.Id = Guid.NewGuid(); // Set the ID property
            await _repository.AddAsync(timezone);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<TimezoneDto>(timezone);
            return new TimezoneResponseDto
            {
                Timezone = responseDto,
                StatusCode = 201,
                Message = "Timezone created successfully"
            };
        }
    }
}