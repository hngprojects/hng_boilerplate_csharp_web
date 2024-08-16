using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<CreateUserLoginCommand, UserLoginResponseDto<SignupResponseData>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<LastLogin> _loginLast;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserCommandHandler(
            IRepository<User> userRepo,
            IRepository<LastLogin> loginLast,
             IMapper mapper,
            IPasswordService passwordService,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _loginLast = loginLast;
            _mapper = mapper;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<UserLoginResponseDto<SignupResponseData>> Handle(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo
                .GetQueryableBySpec(u => u.Email == request.LoginRequestBody.Email)
                .Include(u => u.Organizations)
                .ThenInclude(o => o.UsersRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync();

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


            var lastlogin = new LastLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()

            };

            await _loginLast.AddAsync(lastlogin);
            await _loginLast.SaveChanges();



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
                Role = o.UsersRoles.FirstOrDefault()?.Role.Name,
                IsOwner = o.OwnerId == user.Id,
            }).ToList();
            var signUpResponseData = new SignupResponseData { User = userResponse, Organization = orgs };
            return signUpResponseData;
        }
    }
}