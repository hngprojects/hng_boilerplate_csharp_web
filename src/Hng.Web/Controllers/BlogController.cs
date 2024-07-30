using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/blogs")]
public class BlogController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] CreateBlogDto body)
    {
        var command = new CreateBlogCommand(body);
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBlog), response);
    }
}