using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Hng.Web.Controllers
{
    [Route("logs")]
    public class LogsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public LogsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            if (!_env.IsDevelopment())
            {
                return Forbid();
            }

            var logFilePath = "/etc/logs/Hng.Web.dev/logfile.log";
            if (!System.IO.File.Exists(logFilePath))
            {
                return NotFound("Log file not found.");
            }

            var logContents = System.IO.File.ReadAllText(logFilePath, Encoding.UTF8);
            return Content(logContents, "text/plain");
        }
    }
}

