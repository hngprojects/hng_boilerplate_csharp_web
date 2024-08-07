using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record UpdateProfileDto : IRequest<Result<ProfileDto>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
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
}
