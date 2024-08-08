using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[ApiController]
[Route("api/v1/blogs")]
public class BlogController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] CreateBlogDto body)
    {
        var command = new CreateBlogCommand(body);
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBlog), response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BlogDto>> GetBlogById(Guid id)
    {
        var query = new GetBlogByIdQuery(id);
        var response = await _mediator.Send(query);

        return response is null ? NotFound(new FailureResponseDto<BlogDto>
        {
            Data = null,
            Error = "Blog not found",
            Message = "The requested job does not exist."
        }) : Ok(response);
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(SuccessResponseDto<IEnumerable<BlogDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<SuccessResponseDto<IEnumerable<BlogDto>>>> GetBlogs()
    {
        var blogs = await _mediator.Send(new GetBlogsQuery());
        var response = new SuccessResponseDto<IEnumerable<BlogDto>>
        {
            Data = blogs
        };
        return Ok(response);
    }


    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBlogById(Guid id)
    {
        await _mediator.Send(new DeleteBlogByIdCommand(id));
        return NoContent();
    }
    
    [Authorize]
    [HttpPut("{id:guid}")]
         [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status401Unauthorized)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<BlogDto>> UpdateBlog(Guid id, [FromBody] UpdateBlogDto updateBlogDto)
         {
             var command = new UpdateBlogCommand(updateBlogDto, id);
             var result = await _mediator.Send(command);
             return Ok(result);
         }

    

}