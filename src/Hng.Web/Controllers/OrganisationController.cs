using System.Security.Claims;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/organisations")]
    [Authorize]
    public class OrganisationController(IOrganisationService organisationService) : ControllerBase
    {
        private readonly IOrganisationService organisationService = organisationService;
        
        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromBody] OrganizationDto organisationDto)
        {
            // Validate the model
            var validationResults = new List<ValidationResult>();
            if (string.IsNullOrEmpty(organisationDto.Name))
            {
                validationResults.Add(new ValidationResult("Name must not be null", new[] { "Name" }));
            }
            if (validationResults.Count > 0)
            {
                var errors = validationResults.Select(result => new
                {
                    field = result.MemberNames.FirstOrDefault(),
                    message = result.ErrorMessage
                });
                return UnprocessableEntity(new { errors });
            }

            if(!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new
                {
                    status = "Unauthorized",
                    message = "User not authenticated",
                    statusCode = 401
                });
            }
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var organisation = await organisationService.CreateOrganisationAsync(organisationDto);
                return Created("", new
                {
                    status = "success",
                    message = "Organisation created successfully",
                    data = new
                    {
                        id = organisation.Id,
                        name = organisation.Name,
                        description = organisation.Description
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "Bad request",
                    message = "Client error",
                    statusCode = 400
                });
            }
        }

    }
}