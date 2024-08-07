using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Hng.Application.Utils;

namespace Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services
{
    public class ImageService(CloudinarySettings cloudSettings) : IImageService
    {
        private readonly CloudinarySettings _cloudSettings = cloudSettings;
        private readonly CloudinaryDotNet.Cloudinary _cloudinary = new(new Account(cloudSettings.CloudName, cloudSettings.ApiKey, cloudSettings.ApiSecret));

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    await using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "HNG Bioler Plate",
                        Invalidate = true
                    };
                    _cloudinary.Api.Secure = true;
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl.ToString();
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string url)
        {
            var publicId = GetPublicIdFromUrl(url);
            var deleteParams = new DeletionParams(publicId);
            _cloudinary.Api.Secure = true;
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

        private static string GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var publicIdWithExtension = segments.Last();
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
            return publicId;
        }
    }
}
