// using AutoMapper;
// using Hng.Application.Features.OrganisationInvite.Commands;
// using Hng.Application.Features.OrganisationInvite.Dtos;
// using Hng.Domain.Entities;
// using MediatR;
// using Hng.Application.Shared.Dtos;
// using Microsoft.AspNetCore.Http;
// using Hng.Infrastructure.Utilities;
// using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;
// using Hng.Infrastructure.Services.Interfaces;

// namespace Hng.Application.Features.OrganisationInvite.Handlers;

// public class CreateOrganizationInviteCommandHandler(IOrganisationInviteService inviteService, IMapper mapper) : IRequestHandler<CreateOrganizationInviteCommand, StatusCodeResponse<OrganizationInviteDto>>
// {
//     private readonly IOrganisationInviteService inviteService = inviteService;

//     public async Task<StatusCodeResponse<OrganizationInviteDto>> Handle(CreateOrganizationInviteCommand request, CancellationToken cancellationToken)
//     {
//         if (!Guid.TryParse(request.InviteDto.OrganizationId, out Guid orgId))
//         {
//             return new StatusCodeResponse<OrganizationInviteDto>()
//             {
//                 StatusCode = StatusCodes.Status400BadRequest,
//                 Message = "An invalid organisation id was passed"
//             };
//         }

//         Result<OrganizationInvite> inviteResult = await inviteService.CreateInvite(request.InviteDto.UserId, orgId, request.InviteDto.Email);

//         if (!inviteResult.IsSuccess)
//         {
//             return CreateFailureResponse(inviteResult.Error);
//         }
//         var dto = mapper.Map<OrganizationInviteDto>(inviteResult.Value);
//         return new StatusCodeResponse<OrganizationInviteDto>()
//         {
//             Message = $"Invite for {request.InviteDto.Email} created successfully",
//             StatusCode = StatusCodes.Status201Created,
//             Data = new { invite_link = dto }
//         };
//     }

//     private static StatusCodeResponse<OrganizationInviteDto> CreateFailureResponse(Error error)
//     {
//         return new StatusCodeResponse<OrganizationInviteDto>
//         {
//             Message = error.Message,
//             StatusCode = error switch
//             {
//                 InviteAlreadyExistsError => StatusCodes.Status409Conflict,
//                 OrganisationDoesNotExistError => StatusCodes.Status404NotFound,
//                 UserIsNotOwnerError => StatusCodes.Status401Unauthorized,
//                 _ => StatusCodes.Status422UnprocessableEntity
//             }
//         };
//     }


// }


