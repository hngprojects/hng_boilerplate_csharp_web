using Microsoft.AspNetCore.Mvc;
using Hng.Domain.Entities;
using Hng.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;
using Hng.Infrastructure.Context;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly MyDBContext _context;

        public PermissionsController(MyDBContext context)
        {
            _context = context;
        }


        [SwaggerOperation(
        Summary = "List of permissions Endpoint",
        Description = "Gets the list of permissions",
        Tags = new[] { "PermissionEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<IEnumerable<Permission>> GetPermissions()
        {
            return _context.Permissions.ToList();
        }


        [SwaggerOperation(
        Summary = "Create permission Endpoint",
        Description = "Creates a new permission",
        Tags = new[] { "PermissionEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<Permission> CreatePermission(Permission permission)
        {
            _context.Permissions.Add(permission);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, permission);
        }


        [SwaggerOperation(
         Summary = "Get permission by ID Endpoint",
         Description = "Gets a permission by ID",
         Tags = new[] { "PermissionEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<Permission> GetPermission(Guid id)
        {
            var permission = _context.Permissions.Find(id);
            if (permission == null)
            {
                return NotFound();
            }
            return permission;
        }

 
        [SwaggerOperation(
          Summary = "Update permission Endpoint",
          Description = "Updates an existing permission",
          Tags = new[] { "PermissionEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public IActionResult EditPermission(Guid id, Permission updatedPermission)
        {
            var permission = _context.Permissions.Find(id);
            if (permission == null)
            {
                return NotFound();
            }

            permission.Name = updatedPermission.Name;
            permission.UpdatedAt = DateTime.UtcNow;

            _context.Permissions.Update(permission);
            _context.SaveChanges();

            return NoContent();
        }


        [SwaggerOperation(
          Summary = "Delete permission Endpoint",
          Description = "Deletes a permission by ID",
          Tags = new[] { "PermissionEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult DeletePermission(Guid id)
        {
            var permission = _context.Permissions.Find(id);
            if (permission == null)
            {
                return NotFound();
            }

            _context.Permissions.Remove(permission);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
