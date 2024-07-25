using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, SignUpResponse>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserSignUpCommandHandler> _logger;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;


        public UserSignUpCommandHandler(IRepository<User> userRepository, IMapper mapper, ILogger<UserSignUpCommandHandler> logger, IPasswordService passwordService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }


        public async Task<SignUpResponse> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isUnique = await _userRepository.GetBySpec(u => u.Email == request.SignUpBody.Email);
                if (isUnique is not null)
                {
                    return new SignUpResponse
                    {
                        Message = "Email already exists",
                    };
                }



                var createdUser = _mapper.Map<User>(request.SignUpBody);
                (createdUser.PasswordSalt, createdUser.Password) = _passwordService.GeneratePasswordSaltAndHash(request.SignUpBody.Password);

                await _userRepository.AddAsync(createdUser);
                await _userRepository.SaveChanges();

                var token = _tokenService.GenerateJwt(createdUser);

                return new SignUpResponse
                {
                    Message = "User registered successfully",
                    Data = new SignupResponseData
                    {
                        Token = token,
                        User = _mapper.Map<UserResponseDto>(createdUser),
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user");
                return new SignUpResponse
                {
                    Message = "An error occurred while processing your request",
                };
            }
        }
    }
}
