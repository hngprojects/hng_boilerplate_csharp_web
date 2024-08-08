using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services
{
    public interface IImageService
    {
        Task<bool> DeleteImageAsync(string url);
        Task<string> UploadImageAsync(IFormFile file);
    }
}