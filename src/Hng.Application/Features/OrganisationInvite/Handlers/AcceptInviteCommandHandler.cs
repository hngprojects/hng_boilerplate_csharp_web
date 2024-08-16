using System.Text.Json;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class AcceptInviteCommandHandler(
    IRepository<OrganizationInvite> repository,
    IRepository<User> userRepository,
    IRepository<Organization> orgRepository,
    ILogger<AcceptInviteCommandHandler> logger) : IRequestHandler<AcceptInviteCommand, StatusCodeResponse>
{
    private readonly IRepository<OrganizationInvite> repository = repository;
    private readonly IRepository<User> userRepository = userRepository;
    private readonly IRepository<Organization> orgRepository = orgRepository;
    private readonly ILogger<AcceptInviteCommandHandler> logger = logger;
    private static readonly char[] separator = ['?'];

    public async Task<StatusCodeResponse> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("CODE: {code}", JsonSerializer.Serialize(request));
        var invite = await repository.GetBySpec(e => true);

        var invites = await repository.GetAllBySpec(e => e.InviteLink != "string");
        var existingInvite = invites.Where(e => e.InviteLink.Split(separator)[1].Equals(request.InviteCode.Token)).First();
        logger.LogInformation("Invite Request: {invite}", JsonSerializer.Serialize(request));
        if (existingInvite == null)
        {
            return new StatusCodeResponse { Message = "Invalid invite code provided", StatusCode = StatusCodes.Status422UnprocessableEntity };
        }

        var existingUser = await userRepository.GetBySpec(e => e.Email.Equals(existingInvite.Email));

        if (existingUser == null)
        {
            return new StatusCodeResponse { Message = "Success! Please create an account to be added to the organisation", StatusCode = StatusCodes.Status202Accepted };
        }

        Organization org = await orgRepository.GetAsync(existingInvite.OrganizationId);

        org.Users.Add(existingUser);

        return new StatusCodeResponse { Message = $"You have been added to {org.Name}", StatusCode = StatusCodes.Status200OK };
    }
}
