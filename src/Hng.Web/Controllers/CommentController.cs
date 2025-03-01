using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/posts/{blogId:guid}/comments")]
public class CommentController(IMediator mediator, IAuthenticationService authenticationService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IAuthenticationService _authenticationService = authenticationService;


    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CommentDto>> CreateComment([FromRoute] Guid blogId, [FromBody] CreateCommentDto body)
    {
        var command = new CreateCommentCommand(body, blogId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByBlogId([FromRoute] Guid blogId)
    {
        var query = new GetCommentsByBlogIdQuery(blogId);
        var comments = await _mediator.Send(query);
        var response = new SuccessResponseDto<IEnumerable<CommentDto>>
        {
            Data = comments
        };
        return Ok(response);
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentDto>> UpdateComment(
    [FromRoute] Guid blogId,
    [FromRoute] Guid commentId,
    [FromBody] UpdateCommentDto body)
    {
        var userId = await _authenticationService.GetCurrentUserAsync();
        var command = new UpdateCommentCommand(blogId, commentId, body, userId);
        var updatedComment = await _mediator.Send(command);

        return Ok(updatedComment);
    }
}