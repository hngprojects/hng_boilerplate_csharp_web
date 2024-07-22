using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Application.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IMapper _mapper;

        public OrganisationService(IOrganisationRepository organisationRepository, IMapper mapper)
        {
            _organisationRepository = organisationRepository;
            _mapper = mapper;
        }

        public async Task<Organization> CreateOrganisationAsync(OrganizationDto dto)
        {
            var org = _mapper.Map<Organization>(dto);
            var organisation = await _organisationRepository.AddOrganisation(org);
            return organisation;

        }
    }
}
