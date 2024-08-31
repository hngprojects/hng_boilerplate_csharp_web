using Hng.Application.Features.Squeeze.Dtos;

namespace Hng.Application.Features.Squeeze.Mappers;

public class SqueezeMapperProfile : AutoMapper.Profile
{
    public SqueezeMapperProfile()
    {
        CreateMap<Domain.Entities.Squeeze, SqueezeDto>()
            .ReverseMap();
        CreateMap<CreateSqueezeRequestDto, Domain.Entities.Squeeze>();
        CreateMap<Domain.Entities.Squeeze, SqueezeDto>();
    }
}