﻿using CSharpFunctionalExtensions;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public record DeleteProfilePictureDto : IRequest<Result<DeleteProfilePictureResponseDto>>
    {
    }

    public record DeleteProfilePictureResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }
}