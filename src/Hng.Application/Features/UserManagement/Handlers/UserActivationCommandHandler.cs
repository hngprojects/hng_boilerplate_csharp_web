using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Handlers;

public class UserActivationCommandHandler : IRequestHandler<UserActivationCommand, UserActivationResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserActivationCommandHandler(IRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserActivationResponse> Handle(UserActivationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetBySpec(u => u.Id == request.UserId);

        if (user is null)
        {
            return new UserActivationResponse
            {
                Message = "User not found.",
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        if (user.UserStatus == UserStatus.activate)
        {
            return new UserActivationResponse
            {
                Message = "User is already active.",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        user.UserStatus = UserStatus.activate;

        try
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChanges();

            var userDto = _mapper.Map<UserResponseDto>(user);

            return new UserActivationResponse
            {
                Message = "User activated successfully.",
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception ex)
        {
            // Log the exception
            return new UserActivationResponse
            {
                Message = "An error occurred while activating the user.",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}