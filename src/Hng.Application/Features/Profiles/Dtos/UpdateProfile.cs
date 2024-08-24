using CSharpFunctionalExtensions;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record UpdateProfile : IRequest<Result<UpdateProfileResponseDto>>
    {
        public UpdateProfileDto UpdateProfileDto { get; set; }
    }

    public record UpdateProfileDto
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [JsonPropertyName("pronoun")]
        public string Pronoun { get; set; }

        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("facebook_link")]
        public string FacebookLink { get; set; }

        [JsonPropertyName("instagram_link")]
        public string InstagramLink { get; set; }

        [JsonPropertyName("twitter_link")]
        public string TwitterLink { get; set; }

        [JsonPropertyName("linkedin_link")]
        public string LinkedinLink { get; set; }
    }

    public record UpdateProfileResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public ProfileDto Data { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }
}
