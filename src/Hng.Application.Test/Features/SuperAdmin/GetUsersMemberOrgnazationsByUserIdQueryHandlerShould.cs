using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Handlers;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

public class GetUsersMemberOrganizationsByUserIdQueryHandlerShould
{
    private readonly Mock<IRepository<User>> _mockRepository;
    private readonly IMapper _mapper;
    private readonly GetUsersMemberOrganizationsByUserIdQueryHandler handler;

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
        handler = new GetUsersMemberOrganizationsByUserIdQueryHandler(_mockRepository.Object, _mapper);
    }

    [Fact]
    public async Task ReturnNullWhenUserIsNotFound()
    {
        _mockRepository.Setup(repo => repo.GetBySpec(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Expression<Func<User, object>>[]>()
        )).ReturnsAsync((User)null);

        var result = await handler.Handle(
            new GetUsersMemberOrganizationsByUserIdQuery(Guid.NewGuid(), new BaseQueryParameters()), 
            CancellationToken.None
        );

        Assert.Null(result);
    }
}