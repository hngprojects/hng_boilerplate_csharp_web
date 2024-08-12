using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class UpdateProfilePictureHandler : IRequestHandler<UpdateProfilePictureDto, Result<UpdateProfilePictureResponseDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IImageService _imageService;

        public UpdateProfilePictureHandler(
            IRepository<User> userRepo,
            IImageService imageService)
        {
            _userRepo = userRepo;
            _imageService = imageService;
        }

        public async Task<Result<UpdateProfilePictureResponseDto>> Handle(UpdateProfilePictureDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Id == request.UserId, u => u.Profile);

            if (user == null)
                return Result.Failure<UpdateProfilePictureResponseDto>("User with Email does not Exist!");

            if (user?.Profile == null)
                return Result.Failure<UpdateProfilePictureResponseDto>("Please update your profile!");

            if (request.DisplayPhoto != null)
            {
                if (request.DisplayPhoto.Length != 0 &&
                    (request.DisplayPhoto.ContentType == "image/jpeg" || request.DisplayPhoto.ContentType == "image/png"))
                    return Result.Failure<UpdateProfilePictureResponseDto>("Logo can only be jpeg or png format");

                request.AvatarUrl = await _imageService.UploadImageAsync(request.DisplayPhoto);

                if (!string.IsNullOrWhiteSpace(user.Profile?.AvatarUrl))
                    await _imageService.DeleteImageAsync(user.Profile?.AvatarUrl);
            }

            if (request.DisplayPhoto == null && string.IsNullOrWhiteSpace(request.AvatarUrl))
                if (!string.IsNullOrWhiteSpace(user.Profile?.AvatarUrl))
                    await _imageService.DeleteImageAsync(user.Profile?.AvatarUrl);

            user.Profile.AvatarUrl = request.AvatarUrl;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new UpdateProfilePictureResponseDto()
            {
                Message = "Successful!",
                StatusCode = StatusCodes.Status200OK,
                Data = new UpdateProfilePictureResponse()
                {
                    AvatarUrl = request.AvatarUrl
                }
            });
        }
    }
}