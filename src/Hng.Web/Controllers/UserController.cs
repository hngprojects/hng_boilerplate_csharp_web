using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repo;

        public UserController(IUserRepository repo)
        {
            this.repo = repo;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var dataFromRepo = await repo.GetUserById(id);
            if(dataFromRepo == null)
            {
                return NotFound(new {
                    message = "User not found",
                    is_successful = false,
                    status_code = 404
                });
            }

            return Ok(dataFromRepo);
        }
    }
}