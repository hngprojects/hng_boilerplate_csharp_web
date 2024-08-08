using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Timezones.Handlers.Commands
{
    public class CreateTimezoneCommandHandler : IRequestHandler<CreateTimezoneCommand, CreateTimezoneResponseDto>
    {
        private readonly IRepository<Timezone> _repository;
        private readonly IMapper _mapper;

        public CreateTimezoneCommandHandler(IRepository<Timezone> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateTimezoneResponseDto> Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
        {
            // Check if the timezone already exists
            var existingTimezone = await _repository.GetBySpec(t => t.TimezoneValue == request.Timezone);
            if (existingTimezone != null)
            {
                return new CreateTimezoneResponseDto
                {
                    StatusCode = 409,
                    Error = $"Timezone '{request.Timezone}' already exists.",
                    Message = "Timezone already exists"
                };
            }

            var timezone = _mapper.Map<Timezone>(request);
            await _repository.AddAsync(timezone);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<TimezoneDto>(timezone);
            return new CreateTimezoneResponseDto
            {
                Timezone = responseDto,
                StatusCode = 201,
                Message = "Timezone created successfully"
            };
        }
    }
}