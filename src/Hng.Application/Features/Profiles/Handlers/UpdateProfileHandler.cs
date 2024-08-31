using AutoMapper;
using CSharpFunctionalExtensions;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Profile = Hng.Domain.Entities.Profile;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfile, Result<UpdateProfileResponseDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Profile> _profileRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UpdateProfileHandler(
            IRepository<User> userRepo,
            IRepository<Profile> profileRepo,
            IMapper mapper,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _profileRepo = profileRepo;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<UpdateProfileResponseDto>> Handle(UpdateProfile request, CancellationToken cancellationToken)
        {
            var email = _tokenService.GetCurrentUserEmail();

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<UpdateProfileResponseDto>("Unauthorize user");

            var user = await _userRepo.GetBySpec(u => u.Email == email, u => u.Profile);

            if (user.Profile == null)
            {
                user.Profile = BuildProfile(request.UpdateProfileDto, user.Id);
                user = UpdateUser(user, request.UpdateProfileDto);

                await _profileRepo.AddAsync(user.Profile);
                await _userRepo.UpdateAsync(user);
            }
            else
            {
                user.Profile = UpdateProfile(user, request.UpdateProfileDto);
                user = UpdateUser(user, request.UpdateProfileDto);

                await _userRepo.UpdateAsync(user);
            }
            await _userRepo.SaveChanges();

            var profileDto = _mapper.Map<ProfileDto>(user.Profile);

            return Result.Success(new UpdateProfileResponseDto()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Successful",
                Data = profileDto
            });
        }

        private static User UpdateUser(User user, UpdateProfileDto request)
        {
            user.PhoneNumber = !string.IsNullOrWhiteSpace(request.PhoneNumber) ? request.PhoneNumber : "";
            user.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : "";
            user.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : "";
            user.UpdatedAt = DateTime.UtcNow;
            return user;
        }

        private static Profile UpdateProfile(User user, UpdateProfileDto request)
        {
            user.Profile.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : "";
            user.Profile.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : "";
            user.Profile.Bio = !string.IsNullOrWhiteSpace(request.Bio) ? request.Bio : "";
            user.Profile.Department = !string.IsNullOrWhiteSpace(request.Department) ? request.Department : "";
            user.Profile.FacebookLink = !string.IsNullOrWhiteSpace(request.FacebookLink) ? request.FacebookLink : "";
            user.Profile.JobTitle = !string.IsNullOrWhiteSpace(request.JobTitle) ? request.JobTitle : "";
            user.Profile.LinkedinLink = !string.IsNullOrWhiteSpace(request.LinkedinLink) ? request.LinkedinLink : "";
            user.Profile.PhoneNumber = !string.IsNullOrWhiteSpace(request.PhoneNumber) ? request.PhoneNumber : "";
            user.Profile.Pronoun = !string.IsNullOrWhiteSpace(request.Pronoun) ? request.Pronoun : "";
            user.Profile.TwitterLink = !string.IsNullOrWhiteSpace(request.TwitterLink) ? request.TwitterLink : "";
            user.Profile.InstagramLink = !string.IsNullOrWhiteSpace(request.InstagramLink) ? request.InstagramLink : "";
            user.Profile.Username = !string.IsNullOrWhiteSpace(request.Username) ? request.Username : "";

            return user.Profile;
        }

        private static Profile BuildProfile(UpdateProfileDto request, Guid userId)
        {
            return new Profile()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Bio = request.Bio,
                FacebookLink = request.FacebookLink,
                JobTitle = request.JobTitle,
                LinkedinLink = request.LinkedinLink,
                PhoneNumber = request.PhoneNumber,
                Pronoun = request.Pronoun,
                TwitterLink = request.TwitterLink,
                InstagramLink = request.InstagramLink,
                Username = request.Username,
                Department = request.Department,
                UserId = userId
            };
        }
    }
}