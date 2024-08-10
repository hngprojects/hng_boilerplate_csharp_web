using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<CreateUserLoginCommand, UserLoginResponseDto<object>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(
            IRepository<User> userRepo,
            IMapper mapper,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<UserLoginResponseDto<object>> Handle(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.LoginRequestBody.Email);
            if (user == null || !_passwordService.IsPasswordEqual(request.LoginRequestBody.Password, user.PasswordSalt, user.Password))
            {
                return new UserLoginResponseDto<object>
                {
                    Data = null,
                    AccessToken = null,
                    Message = "Invalid credentials"
                };
            }

            var token = _tokenService.GenerateJwt(user);

            var userDto = _mapper.Map<UserResponseDto>(user);

            return new UserLoginResponseDto<object>
            {
                Data = userDto,
                AccessToken = token,
                Message = "Login successful"
            };
        }
    }
}