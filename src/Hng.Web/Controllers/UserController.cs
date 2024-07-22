using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Application.Interfaces;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var dataFromRepo = await userService.GetUserByIdAsync(id);
            if (dataFromRepo == null)
            {
                return NotFound(new
                {
                    message = "User not found",
                    is_successful = false,
                    status_code = 404
                });
            }

            return Ok(dataFromRepo);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }
        [Route("api/v1/organizations/{org_id}/users")]

        [HttpGet]
        public async Task<IActionResult> GetUsersByStatus(string org_id, [FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            if (!new[] { "members", "suspended", "left" }.Contains(status.ToLower()))
            {
                return BadRequest(new { message = "Invalid status value", statusCode = 400 });
            }

            var users = await userService.GetUsersByStatusAsync(org_id, status, page, limit);
            if (!users.Any())
            {
                return NotFound(new { message = "No users found", statusCode = 404 });
            }

            return Ok(new
            {
                total = users.Count(),
                page,
                limit,
                prev = page > 1 ? $"/api/v1/organizations/{org_id}/users?status={status}&page={page - 1}&limit={limit}" : null,
                next = users.Count() == limit ? $"/api/v1/organizations/{org_id}/users?status={status}&page={page + 1}&limit={limit}" : null,
                users
            });



        }
    }
}
