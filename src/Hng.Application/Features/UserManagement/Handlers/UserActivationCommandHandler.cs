using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.UserManagement.Handlers;

public class UserActivationCommandHandler(IRepository<User> userRepository) : IRequestHandler<UserActivationCommand, UserActivationResponse>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper;

    public UserActivationCommandHandler(IRepository<User> userRepository, IMapper mapper) : this(userRepository)
    {
        _mapper = mapper;
    }

    public async Task<UserActivationResponse> Handle(UserActivationCommand request, CancellationToken cancellationToken)
    {
       
        var user = await _userRepository.GetBySpec(u => u.Id == request.UserId );

        if (user is null)
        {
            return new UserActivationResponse
            {
                Message = "Organization not found.",
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        if (user.UserStatus == Domain.Enums.UserStatus.activate) 
        {
            return new UserActivationResponse
            {
                Message = "User Already Active.",
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        user.UserStatus = Domain.Enums.UserStatus.activate;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChanges();
        var User = _mapper.Map<UserResponseDto>(user);
        return new UserActivationResponse
        {
            Message = "User activated successfully",
            StatusCode = StatusCodes.Status200OK,
            User = User
        };
    }
}