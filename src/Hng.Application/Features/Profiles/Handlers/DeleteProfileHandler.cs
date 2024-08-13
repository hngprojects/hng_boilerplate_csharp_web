using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Profiles.Handlers
{
    public class DeleteProfileHandler : IRequestHandler<DeleteProfilePictureDto, Result<DeleteProfilePictureResponseDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IImageService _imageService;

        public DeleteProfileHandler(IRepository<User> userRepo, IImageService imageService)
        {
            _userRepo = userRepo;
            _imageService = imageService;
        }

        public async Task<Result<DeleteProfilePictureResponseDto>> Handle(DeleteProfilePictureDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.Email, u => u.Profile);

            if (user == null)
                return Result.Failure<DeleteProfilePictureResponseDto>("User with Email does not Exist!");

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