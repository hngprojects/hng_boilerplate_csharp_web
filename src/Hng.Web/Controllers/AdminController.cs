using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Admin: Users - gets all registered users by search parameters
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUsersBySearch([FromQuery] UsersQueryParameters parameters)
        {
            var users = await _mediator.Send(new GetUsersBySearchQuery(parameters));
            return Ok(new PaginatedResponseDto<PagedListDto<UserSuperDto>> { Data = users, Metadata = users.MetaData });
        }

        /// <summary>
        /// Admin: Users - gets all organizations owned by a user by the user's id
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/{id}/organizations/owned")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<PaginatedResponseDto<PagedListDto<OrganizationDto>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(StatusCodeResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserOwnedOrganizationsByUserId([FromRoute] Guid id, [FromQuery] BaseQueryParameters parameters)
        {
            if (!Guid.TryParse(id.ToString(), out _))
            {
                return BadRequest(new FailureResponseDto<object> { Message = "Valid user ID must be Provided" });
            }

            var userOrganizations = await _mediator.Send(new GetUsersOwnedOrganizationsByUserIdQuery(id, parameters));

            if (userOrganizations == null)
            {
                return NotFound(new StatusCodeResponse
                {
                    Message = "User not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(new PaginatedResponseDto<PagedListDto<OrganizationDto>> { Data = userOrganizations, Metadata = userOrganizations.MetaData });
        }

        /// <summary>
        /// Admin: Users - gets all organizations a user belongs by the user's id
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/{id}/organizations/member")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<PaginatedResponseDto<PagedListDto<OrganizationDto>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(StatusCodeResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserMemberOrganizationsByUserId([FromRoute] Guid id, [FromQuery] BaseQueryParameters parameters)
        {
            if (!Guid.TryParse(id.ToString(), out _))
            {
                return BadRequest(new FailureResponseDto<object> { Message = "Valid user ID must be Provided" });
            }

            var userOrganizations = await _mediator.Send(new GetUsersMemberOrganizationsByUserIdQuery(id, parameters));

            if (userOrganizations == null)
            {
                return NotFound(new StatusCodeResponse
                {
                    Message = "User not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(new PaginatedResponseDto<PagedListDto<OrganizationDto>> { Data = userOrganizations, Metadata = userOrganizations.MetaData });
        }
    }
}
