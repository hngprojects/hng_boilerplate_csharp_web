using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Profile = Hng.Domain.Entities.Profile;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class UpdateProfilePictureHandler : IRequestHandler<UpdateProfilePictureDto, Result<UpdateProfilePictureResponseDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IImageService _imageService;
        private readonly IRepository<Profile> _profileRepo;

        public UpdateProfilePictureHandler(
            IRepository<User> userRepo,
            IImageService imageService,
            IRepository<Profile> profileRepo)
        {
            _userRepo = userRepo;
            _imageService = imageService;
            _profileRepo = profileRepo;
        }

        public async Task<Result<UpdateProfilePictureResponseDto>> Handle(UpdateProfilePictureDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.Email, u => u.Profile);

            if (user == null)
                return Result.Failure<UpdateProfilePictureResponseDto>("User with Email does not Exist!");

            if (request.DisplayPhoto != null)
            {
                if (request.DisplayPhoto.Length != 0 &&
                    request.DisplayPhoto.ContentType != "image/jpeg" && request.DisplayPhoto.ContentType != "image/png")
                    return Result.Failure<UpdateProfilePictureResponseDto>("Logo can only be jpeg or png format");

                var avatarUrl = await _imageService.UploadImageAsync(request.DisplayPhoto);

                if (user.Profile == null)
                {
                    user.Profile = new Profile() { UserId = user.Id, AvatarUrl = avatarUrl };
                    user.AvatarUrl = avatarUrl;

                    await _profileRepo.AddAsync(user.Profile);
                    await _userRepo.UpdateAsync(user);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(user.Profile?.AvatarUrl))
                        await _imageService.DeleteImageAsync(user.Profile?.AvatarUrl);

                    user.Profile.AvatarUrl = avatarUrl;
                    user.AvatarUrl = avatarUrl;

                    await _userRepo.UpdateAsync(user);
                }
            }

            await _userRepo.SaveChanges();

            return Result.Success(new UpdateProfilePictureResponseDto()
            {
                Message = "Successful!",
                StatusCode = StatusCodes.Status200OK,
                Data = new UpdateProfilePictureResponse()
                {
                    AvatarUrl = user.Profile?.AvatarUrl
                }
            });
        }
    }
}