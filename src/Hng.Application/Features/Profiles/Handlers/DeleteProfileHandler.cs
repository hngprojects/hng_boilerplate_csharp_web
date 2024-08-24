using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class DeleteProfileHandler : IRequestHandler<DeleteProfilePictureDto, Result<DeleteProfilePictureResponseDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;

        public DeleteProfileHandler(
            IRepository<User> userRepo,
            IImageService imageService,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _imageService = imageService;
            _tokenService = tokenService;
        }

        public async Task<Result<DeleteProfilePictureResponseDto>> Handle(DeleteProfilePictureDto request, CancellationToken cancellationToken)
        {
            var email = _tokenService.GetCurrentUserEmail();

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<DeleteProfilePictureResponseDto>("Unauthorized user");

            var user = await _userRepo.GetBySpec(u => u.Email == email, u => u.Profile);

            if (!string.IsNullOrWhiteSpace(user?.Profile?.AvatarUrl))
                await _imageService.DeleteImageAsync(user?.Profile?.AvatarUrl);

            user.Profile.AvatarUrl = null;
            user.AvatarUrl = null;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new DeleteProfilePictureResponseDto()
            {
                Message = "Successful",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}