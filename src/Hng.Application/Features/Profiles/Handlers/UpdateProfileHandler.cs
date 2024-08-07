using AutoMapper;
using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Profile = Hng.Domain.Entities.Profile;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileDto, Result<ProfileDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Profile> _profileRepo;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public UpdateProfileHandler(
            IRepository<User> userRepo,
            IRepository<Profile> profileRepo,
            IImageService imageService,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _profileRepo = profileRepo;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<Result<ProfileDto>> Handle(UpdateProfileDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.Email, u => u.Profile);

            if (user == null)
                return Result.Failure<ProfileDto>("User with Email does not Exist!");

            if (request.DisplayPhoto != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Profile?.AvatarUrl))
                    await _imageService.DeleteImageAsync(user.Profile?.AvatarUrl);

                request.AvatarUrl = await _imageService.UploadImageAsync(request.DisplayPhoto);
            }

            if (request.DisplayPhoto == null && string.IsNullOrWhiteSpace(request.AvatarUrl))
                if (!string.IsNullOrWhiteSpace(user.Profile?.AvatarUrl))
                    await _imageService.DeleteImageAsync(user.Profile?.AvatarUrl);

            if (user.Profile == null)
            {
                user.Profile = BuildProfile(request, user.Id);
                user = UpdateUser(user, request);

                await _profileRepo.AddAsync(user.Profile);
                await _userRepo.UpdateAsync(user);
            }
            else
            {
                user.Profile = UpdateProfile(user, request);
                user = UpdateUser(user, request);

                await _userRepo.UpdateAsync(user);
            }

            await _userRepo.SaveChanges();

            var profileDto = _mapper.Map<ProfileDto>(user.Profile);

            return Result.Success(profileDto);
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
            user.Profile.AvatarUrl = !string.IsNullOrWhiteSpace(request.AvatarUrl) ? request.AvatarUrl : "";
            user.Profile.Bio = !string.IsNullOrWhiteSpace(request.Bio) ? request.Bio : "";
            user.Profile.FacebookLink = !string.IsNullOrWhiteSpace(request.FacebookLink) ? request.FacebookLink : "";
            user.Profile.JobTitle = !string.IsNullOrWhiteSpace(request.JobTitle) ? request.JobTitle : "";
            user.Profile.LinkedinLink = !string.IsNullOrWhiteSpace(request.LinkedinLink) ? request.LinkedinLink : "";
            user.Profile.PhoneNumber = !string.IsNullOrWhiteSpace(request.PhoneNumber) ? request.PhoneNumber : "";
            user.Profile.Pronoun = !string.IsNullOrWhiteSpace(request.Pronoun) ? request.Pronoun : "";
            user.Profile.TwitterLink = !string.IsNullOrWhiteSpace(request.TwitterLink) ? request.TwitterLink : "";
            user.Profile.Username = !string.IsNullOrWhiteSpace(request.Username) ? request.Username : "";

            return user.Profile;
        }

        private static Profile BuildProfile(UpdateProfileDto request, Guid userid)
        {
            return new Profile()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                AvatarUrl = request.AvatarUrl,
                Bio = request.Bio,
                FacebookLink = request.FacebookLink,
                JobTitle = request.JobTitle,
                LinkedinLink = request.LinkedinLink,
                PhoneNumber = request.PhoneNumber,
                Pronoun = request.Pronoun,
                TwitterLink = request.TwitterLink,
                Username = request.Username,
                UserId = userid
            };
        }
    }
}
