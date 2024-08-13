using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<CreateUserLoginCommand, UserLoginResponseDto<SignupResponseData>>
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

        public async Task<UserLoginResponseDto<SignupResponseData>> Handle(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.LoginRequestBody.Email, u => u.Organizations);
            if (user == null || !_passwordService.IsPasswordEqual(request.LoginRequestBody.Password, user.PasswordSalt, user.Password))
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    AccessToken = null,
                    Message = "Invalid credentials",
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            var token = _tokenService.GenerateJwt(user);

            return new UserLoginResponseDto<SignupResponseData>
            {
                Data = GetUserDetails(user),
                AccessToken = token,
                Message = "Login successful"
            };
        }

        private SignupResponseData GetUserDetails(User user)
        {
            var userResponse = _mapper.Map<UserResponseDto>(user);
            var orgs = user.Organizations.Select(o => new OrganisationDto
            {
                Id = o.Id,
                Name = o.Name,
                Role = o.UsersRoles.Where(x => x.User == user && x.Orgainzation == o).FirstOrDefault()?.Role.Name,
                IsOwner = o.OwnerId == user.Id,
            }).ToList();
            var signUpResponseData = new SignupResponseData { User = userResponse, Organization = orgs };
            return signUpResponseData;
        }
    }
}