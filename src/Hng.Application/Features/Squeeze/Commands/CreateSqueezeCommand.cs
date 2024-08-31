using Hng.Application.Features.Squeeze.Dtos;
using MediatR;

namespace Hng.Application.Features.Squeeze.Commands;

public class CreateSqueezeCommand(CreateSqueezeRequestDto createSqueezeRequestDto) : IRequest<CreateSqueezeResponseDto>
{
    public CreateSqueezeRequestDto SqueezeBody { get; } = createSqueezeRequestDto;
}