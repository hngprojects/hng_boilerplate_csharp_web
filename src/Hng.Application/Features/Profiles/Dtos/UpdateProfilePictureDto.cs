using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record UpdateProfilePictureDto : IRequest<Result<UpdateProfilePictureResponseDto>>
    {
        public Guid UserId { get; set; }

        public string AvatarUrl { get; set; }

        public IFormFile DisplayPhoto { get; set; }
    }

    public record UpdateProfilePictureResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

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
