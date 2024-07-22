using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hng.Application.Dto;
using Hng.Application.Interfaces;


namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/jobs")]
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
            if (HttpContext.Request.Method != "POST")
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed);
            }

            var createdJobListing = await _jobListingService.CreateJobListingAsync(createJobListingDto);
            return CreatedAtAction(nameof(CreateJobListing), new { id = createdJobListing.Id }, createdJobListing);
        }
    }
}
