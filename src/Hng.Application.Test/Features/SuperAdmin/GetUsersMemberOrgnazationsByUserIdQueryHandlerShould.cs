


using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;

public class GetUsersMemberOrganizationsByUserIdQueryHandlerShould
{
    private readonly Mock<IRepository<User>> _mockRepository;
    private readonly IMapper _mapper;
    public GetUsersMemberOrganizationsByUserIdQueryHandlerShould()
    {
        _mockRepository = new Mock<IRepository<User>>();

        // Set up AutoMapper with your profiles
        var config = new MapperConfiguration(cfg =>
        {
            // Add your AutoMapper profiles here
            cfg.CreateMap<User, UserSuperDto>();
            cfg.CreateMap<Organization, OrganizationDto>();
        });

        _mapper = config.CreateMapper();
    }
}