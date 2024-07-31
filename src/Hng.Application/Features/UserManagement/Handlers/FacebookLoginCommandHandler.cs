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
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommand, SuccessResponseDto<UserLoginResponseDto<UserDto>>>
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

        public async Task<SuccessResponseDto<UserLoginResponseDto<UserDto>>> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var facebookUser = await _facebookAuthService.ValidateAsync(request.FacebookToken);

                if (facebookUser == null)
                {
                    throw new Exception("Invalid Facebook token.");
                }

                var user = await _userRepo.GetBySpec(u => u.Email == facebookUser.Email);
                if (user == null)
                {
                    user = _mapper.Map<User>(facebookUser);
                    await _userRepo.AddAsync(user);
                    await _userRepo.SaveChanges();
                }

                var token = _tokenService.GenerateJwt(user);

                var response = new SuccessResponseDto<UserLoginResponseDto<UserDto>>
                {
                    Data = new UserLoginResponseDto<UserDto>
                    {
                        Data = new UserDto
                        {
                            Id = user.Id,
                            FullName = user.FirstName,
                            Email = user.Email,
                        },
                        AccessToken = token,
                    },
                    Message = "Login successful."
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Facebook login failed: {ErrorMessage}", ex.Message);
                throw new Exception("Login failed.");
            }
        }
    }

}
