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
        private readonly IUserService _userService;
        private readonly ILogger<UserSignUpCommandHandler> _logger;


        public UserSignUpCommandHandler(IRepository<User> userRepository, IMapper mapper, IUserService userService, ILogger<UserSignUpCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }


        public async Task<SignUpResponse> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
        {


            var (isUnique, errorMessage) = await _userService.IsEmailUniqueAsync(request.SignUpBody.Email);
            if (!isUnique)
            {
                return new SignUpResponse
                {
                    Message = errorMessage,
                };
            }

            try
            {
                var createdUser = _mapper.Map<User>(request.SignUpBody);
                createdUser.Password = _userService.HashPassword(request.SignUpBody.Password);

                await _userRepository.AddAsync(createdUser);
                await _userRepository.SaveChanges();

                var token = _userService.GenerateJwtToken(createdUser);

                return new SignUpResponse
                {
                    Message = "User registered successfully",
                    Data = new SignupResponseData
                    {
                        Token = token,
                        User = _mapper.Map<UserResponseDto>(createdUser) ,
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
