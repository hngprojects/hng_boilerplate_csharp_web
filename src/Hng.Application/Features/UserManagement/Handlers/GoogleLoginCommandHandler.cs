using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, UserLoginResponseDto<object>>
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

        public async Task<UserLoginResponseDto<object>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await _googleAuthService.ValidateAsync(request.IdToken);
            }
            catch (InvalidJwtException ex)
            {
                return new UserLoginResponseDto<object>
                {
                    Message = "Invalid Google token.",
                    Data = null
                };
            }

            var dbUser = await _userRepo.GetBySpec(x => x.Email == payload.Email);

            if (dbUser == null)
            {

                var newUser = _mapper.Map<User>(payload);
                newUser.Id = Guid.NewGuid();
                newUser.AvatarUrl = payload.Picture;


                await _userRepo.AddAsync(newUser);
                await _userRepo.SaveChanges();

                var access_token = _tokenService.GenerateJwt(newUser);


                return new UserLoginResponseDto<object>
                {
                    Data = new
                    {
                        user = _mapper.Map<UserDto>(newUser)
                    },
                    AccessToken = access_token,
                    Message = "Registration successful, user logged in."
                };
            }

            var token = _tokenService.GenerateJwt(dbUser);
            var user = _mapper.Map<UserDto>(dbUser);

            return new UserLoginResponseDto<object>
            {
                AccessToken = token,
                Message = "Login successful",
                Data = new { user }
            };
        }
    }

}
