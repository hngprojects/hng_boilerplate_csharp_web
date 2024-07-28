using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, UserLoginResponseDto>
    {
        private readonly IRepository<User> _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleLoginCommandHandler(IRepository<User> userRepo, ITokenService tokenService, IMapper mapper, IGoogleAuthService googleAuthService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _googleAuthService = googleAuthService;
        }

        public async Task<UserLoginResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await _googleAuthService.ValidateAsync(request.IdToken);
            }
            catch (InvalidJwtException ex)
            {
                return new UserLoginResponseDto
                {
                    Message = "Invalid Google token.",
                    Data = null
                };
            }

            var user = await _userRepo.GetBySpec(x => x.Email == payload.Email);

            if (user == null)
            {
                var newUser = _mapper.Map<User>(payload);
                newUser.AvatarUrl = payload.Picture;

                await _userRepo.AddAsync(newUser);

                var userToken = _tokenService.GenerateJwt(newUser);

                return new UserLoginResponseDto
                {
                    Data = _mapper.Map<UserDto>(newUser),
                    AccessToken = userToken,
                    Message = "Registration successful, user logged in."
                };
            }

            var token = _tokenService.GenerateJwt(user);
            var userDto = _mapper.Map<UserDto>(user);

            return new UserLoginResponseDto
            {
                AccessToken = token,
                Message = "Login successful",
                Data = userDto
            };
        }
    }

}
