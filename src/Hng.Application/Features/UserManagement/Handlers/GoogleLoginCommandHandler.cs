using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, UserLoginResponseDto<SignupResponseData>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleLoginCommandHandler(IRepository<User> userRepo, IRepository<Role> roleRepository, ITokenService tokenService, IMapper mapper, IGoogleAuthService googleAuthService)
        {
            _userRepo = userRepo;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _googleAuthService = googleAuthService;
        }

        public async Task<UserLoginResponseDto<SignupResponseData>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await _googleAuthService.ValidateAsync(request.IdToken);
            }
            catch (InvalidJwtException ex)
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Message = "Invalid Google token.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            var dbUser = await _userRepo.GetQueryableBySpec(u => u.Email == payload.Email)
                .Include(u => u.Organizations)
                .ThenInclude(o => o.UsersRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync();

            if (dbUser == null)
            {

                var newUser = _mapper.Map<User>(payload);
                newUser.Id = Guid.NewGuid();
                newUser.AvatarUrl = payload.Picture;

                var userOrg = new Organization
                {
                    Name = $"{newUser.FirstName}'s Org",
                    OwnerId = newUser.Id,
                    Email = newUser.Email,
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                };
                newUser.Organizations.Add(userOrg);
                var sub = new Subscription
                {
                    Frequency = SubscriptionFrequency.Annually,
                    IsActive = true,
                    Plan = SubscriptionPlan.Free,
                    StartDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    UserId = newUser.Id,
                    OrganizationId = userOrg.Id,
                    Amount = 0
                };
                newUser.Subscriptions.Add(sub);
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = userOrg.Id,
                    Name = "Admin",
                    IsActive = true
                };
                role.UsersRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Id,
                    OrganizationId = userOrg.Id
                });
                await _userRepo.AddAsync(newUser);
                await _roleRepository.AddAsync(role);
                await _userRepo.SaveChanges();

                var access_token = _tokenService.GenerateJwt(newUser);
                SignupResponseData signUpResponseData = GetUserDetails(newUser);

                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = signUpResponseData,
                    AccessToken = access_token,
                    Message = "Registration successful, user logged in."
                };
            }

            var token = _tokenService.GenerateJwt(dbUser);

            return new UserLoginResponseDto<SignupResponseData>
            {
                AccessToken = token,
                Message = "Login successful",
                Data = GetUserDetails(dbUser)
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
            var subs = user.Subscriptions.Select(r => new SubscribeFreePlanResponse
            {
                SubscriptionId = r.Id,
                Frequency = r.Frequency.ToString(),
                IsActive = r.IsActive,
                Plan = r.Plan.ToString(),
                StartDate = r.StartDate,
                UserId = r.UserId,
                OrganizationId = r.OrganizationId,
                Amount = r.Amount,
            }).ToList();
            var signUpResponseData = new SignupResponseData { User = userResponse, Organization = orgs, Subscription = subs };
            return signUpResponseData;
        }
    }
}
