using AutoMapper;
using Hng.Application.Features.Squeeze.Commands;
using Hng.Application.Features.Squeeze.Dtos;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Squeeze.Handlers;

public class CreateSqueezeCommandHandler(IRepository<Domain.Entities.Squeeze> squeezeRepository, IMapper mapper) : IRequestHandler<CreateSqueezeCommand, CreateSqueezeResponseDto>
{
    private readonly IRepository<Domain.Entities.Squeeze> _squeezeRepository = squeezeRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateSqueezeResponseDto> Handle(CreateSqueezeCommand request, CancellationToken cancellationToken)
    {
        var squeeze = _mapper.Map<Domain.Entities.Squeeze>(request.SqueezeBody);

        await _squeezeRepository.AddAsync(squeeze);
        await _squeezeRepository.SaveChanges();

        var squeezeDto = _mapper.Map<SqueezeDto>(squeeze);

        return new CreateSqueezeResponseDto()
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Squeeze Created Successfully",
            Data = squeezeDto
        };
    }
}