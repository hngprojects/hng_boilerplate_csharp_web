using Hng.Application.Features.Jobs.Dtos;
using Hng.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Hng.Application.Features.Jobs.Mappers;

public class JobMapperProfile : Profile
{
    public JobMapperProfile()
    {
        CreateMap<CreateJobDto, Job>();
        CreateMap<Job, JobDto>()
            .ReverseMap();
    }
}