using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly MyDBContext _context;

        public BlogsController(MyDBContext context)
        {
            _context = context;
        }


        [SwaggerOperation(
             Summary = "List of blogs Endpoint",
             Description = "Gets the list of blogs",
             Tags = new[] { "BlogEndpoints" })
           ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<IEnumerable<Blog>> GetBlogs()
        {
            return _context.Blogs.ToList();
        }

        [SwaggerOperation(
          Summary = "Create blog Endpoint",
          Description = "Creates a new blog",
          Tags = new[] { "BlogEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<Blog> CreateBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }

        [SwaggerOperation(
          Summary = "Get blog by ID Endpoint",
          Description = "Gets a blog by ID",
          Tags = new[] { "BlogEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<Blog> GetBlog(Guid id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        [SwaggerOperation(
           Summary = "Update blog Endpoint",
           Description = "Updates an existing blog",
           Tags = new[] { "BlogEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public IActionResult EditBlog(Guid id, Blog updatedBlog)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.Title = updatedBlog.Title;
            blog.Author = updatedBlog.Author;
            blog.Body = updatedBlog.Body;
            blog.UpdatedAt = DateTime.UtcNow;

            _context.Blogs.Update(blog);
            _context.SaveChanges();

            return NoContent();
        }

        [SwaggerOperation(
           Summary = "Delete blog Endpoint",
           Description = "Deletes a blog by ID",
           Tags = new[] { "BlogEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(Guid id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
