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
    }
}