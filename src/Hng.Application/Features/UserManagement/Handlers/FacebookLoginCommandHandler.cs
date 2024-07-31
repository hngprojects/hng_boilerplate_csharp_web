using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommand, UserLoginResponseDto<object>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IMapper _mapper;
        private readonly ILogger<FacebookLoginCommandHandler> _logger;

        public FacebookLoginCommandHandler(
            IRepository<User> userRepo,
            ITokenService tokenService,
            IFacebookAuthService facebookAuthService,
            IMapper mapper,
            ILogger<FacebookLoginCommandHandler> logger)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _facebookAuthService = facebookAuthService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserLoginResponseDto<object>> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var facebookUser = await _facebookAuthService.ValidateAsync(request.FacebookToken);

                if (facebookUser == null)
                {
                    return new UserLoginResponseDto<object>
                    {
                        Message = "Invalid Facebook token.",
                        Data = null
                    };
                }

                var user = await _userRepo.GetBySpec(u => u.Email == facebookUser.Email);
                if (user == null)
                {
                    user = _mapper.Map<User>(facebookUser);
                    user.AvatarUrl = facebookUser.Picture.Data.Url;

                    await _userRepo.AddAsync(user);
                    await _userRepo.SaveChanges();

                    var access_token = _tokenService.GenerateJwt(user);

                    return new UserLoginResponseDto<object>
                    {
                        Data = new
                        {
                            user = _mapper.Map<UserDto>(user),
                            access_token
                        },
                        AccessToken = access_token,
                        Message = "Registration successful, user logged in."
                    };
                }

                var token = _tokenService.GenerateJwt(user);
                var userDto = _mapper.Map<UserDto>(user);

                return new UserLoginResponseDto<object>
                {
                    AccessToken = token,
                    Message = "Login successful",
                    Data = new
                    {
                        user = userDto,
                        access_token = token
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Facebook login failed: {ErrorMessage}", ex.Message);
                return new UserLoginResponseDto<object>
                {
                    Message = "Login failed.",
                    Data = null
                };
            }
        }

    }

}
