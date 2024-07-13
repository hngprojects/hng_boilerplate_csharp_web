using Microsoft.AspNetCore.Mvc;
using Hng.Domain.Entities;
using Hng.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using Hng.Infrastructure.Context;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MyDBContext _context;

        public RolesController(MyDBContext context)
        {
            _context = context;
        }

        [SwaggerOperation(
          Summary = "List of roles Endpoint",
          Description = "Gets the list of roles",
          Tags = new[] { "RoleEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            return _context.Roles.ToList();
        }


        [SwaggerOperation(
           Summary = "Create role Endpoint",
           Description = "Creates a new role",
           Tags = new[] { "RoleEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<Role> CreateRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }


        [SwaggerOperation(
          Summary = "Get role by ID Endpoint",
          Description = "Gets a role by ID",
          Tags = new[] { "RoleEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<Role> GetRole(Guid id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            return role;
        }


        [SwaggerOperation(
         Summary = "Update role Endpoint",
         Description = "Updates an existing role",
         Tags = new[] { "RoleEndpoints" })
       ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkResult),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public IActionResult EditRole(Guid id, Role updatedRole)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = updatedRole.Name;
            role.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Update(role);
            _context.SaveChanges();

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Delete role Endpoint",
            Description = "Deletes a role by ID",
            Tags = new[] { "RoleEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(Guid id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
