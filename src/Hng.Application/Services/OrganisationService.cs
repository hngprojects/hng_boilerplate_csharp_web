using Hng.Application.Interfaces;
using AutoMapper;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Application.ViewModel;

namespace Hng.Application.Services
{
	public class OrganisationService(IOrganisationRepository organisationRepository, IMapper mapper)
		: IOrganisationService
	{
		private readonly IMapper _mapper = mapper;

		public async Task<Organization> DeleteOrganizationAsync(Guid id)
		{
			var organization = await organisationRepository.GetAsync(id) ?? throw new KeyNotFoundException("Organization Not Found");

			organization.IsDeleted = true;
			await organisationRepository.UpdateAsync(organization);

			return organization;
		}

		public async Task<OrganisationInviteResponseModel> SendInvites(OrganisationInviteRequestModel request)
		{
			OrganisationInviteResponseModel responseModel = new OrganisationInviteResponseModel();
   
			List<InvitationResponse> response = new List<InvitationResponse>();

			foreach (var item in request.emails)
			{
				//Implement sending of mail using a team 

				response.Add(new InvitationResponse
				{
                 email = item.Email,
				 org_id = item.Id,
				 expires_at = DateTime.Now().AddSeconds(50)
				});
			}
			responseModel.invitations = response;
			responseModel.message = "Invitation(s) sent successfully";

			return responseModel;

		}
	}
}