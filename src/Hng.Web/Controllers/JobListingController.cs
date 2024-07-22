using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using System.Threading.Tasks;

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdJobListing = await _jobListingService.CreateJobListingAsync(createJobListingDto);
                if (createdJobListing == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating job listing");
                }

                return CreatedAtAction(nameof(CreateJobListing), new { id = createdJobListing.Id }, createdJobListing);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if needed
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
