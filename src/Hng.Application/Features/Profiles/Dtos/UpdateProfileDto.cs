using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record UpdateProfileDto : IRequest<Result<UpdateProfileResponseDto>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string AvatarUrl { get; set; }

        public string Username { get; set; }

        public string Pronoun { get; set; }

        public string JobTitle { get; set; }

        public string Bio { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string LinkedinLink { get; set; }

        public IFormFile DisplayPhoto { get; set; }
    }

    public record UpdateProfileResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        public ProfileDto Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }
}
