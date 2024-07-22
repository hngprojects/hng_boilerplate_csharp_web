using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Hng.Web.Controllers
{
    [Route("api/v1/jobs")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobListingService _jobListingService;

        public JobsController(IJobListingService jobListingService)
        {
            _jobListingService = jobListingService;
        }

        [HttpPost]
        public async Task<ActionResult<JobListingDto>> CreateJobListing(CreateJobListingDto createJobListingDto)
        {
            // Check for model state validity
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdJobListing = await _jobListingService.CreateJobListingAsync(createJobListingDto);
                return CreatedAtAction(nameof(CreateJobListing), new { id = createdJobListing.Id }, createdJobListing);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Log the exception details if needed
                return StatusCode(500, "An error occurred while creating the job listing.");
            }
        }
    }
}
