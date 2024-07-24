using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Organisations.Handlers;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
{
    private IRepository<Organization> _organizationRepository;
    private IMapper _mapper;


    public CreateOrganizationCommandHandler(IRepository<Organization> organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = _mapper.Map<Organization>(request.OrganizationBody);

        organization.CreatedAt = DateTime.UtcNow;
        organization.UpdatedAt = DateTime.UtcNow;

        await _organizationRepository.AddAsync(organization);
        await _organizationRepository.SaveChanges();
        return _mapper.Map<OrganizationDto>(organization);
    }
}