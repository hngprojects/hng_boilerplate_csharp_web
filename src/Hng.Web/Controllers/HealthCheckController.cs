using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Health Check Endpoint - Verifies that the API is running.
        /// </summary>
        /// <returns>Returns API health status.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(HealthCheckResponse), 200)]
        [ProducesResponseType(500)]
        public ActionResult<HealthCheckResponse> GetHealthCheck()
        {
            var response = new HealthCheckResponse
            {
                StatusCode = 200,
                Message = "Welcome to C-Sharp Backend Endpoint"
            };

            return new OkObjectResult(response);
        }
    }

    /// <summary>
    /// Represents the response structure for the health check endpoint.
    /// </summary>
    public class HealthCheckResponse
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message indicating the API status.
        /// </summary>
        public string Message { get; set; }
    }
}

