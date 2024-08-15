using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record UpdateProfilePictureDto : UpdateProfilePicture, IRequest<Result<UpdateProfilePictureResponseDto>>
    {
        public UpdateProfilePictureDto(string email, IFormFile photo)
        {
            Email = email;
            DisplayPhoto = photo;
        }

        public string Email { get; set; }
    }

    public record UpdateProfilePicture
    {
        [JsonPropertyName("display_photo")]
        public IFormFile DisplayPhoto { get; set; }
    }

    public record UpdateProfilePictureResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public UpdateProfilePictureResponse Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }

    public record UpdateProfilePictureResponse
    {
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
