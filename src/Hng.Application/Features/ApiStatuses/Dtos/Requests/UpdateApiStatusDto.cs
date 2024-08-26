using CSharpFunctionalExtensions;
using Hng.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.ApiStatuses.Dtos.Requests
{
    public record UpdateApiStatusDto : IRequest<Result<CreateApiStatusResponseDto>>
    {
        [Required(ErrorMessage = "Report is required")]
        public IFormFile report { get; set; }
    }

    public record ApiStatusModel
    {
        [JsonPropertyName("api_group")]
        public string ApiGroup { get; set; }

        [JsonPropertyName("status")]
        public ApiStatusType Status { get; set; }

        [JsonPropertyName("response_time")]
        public long ResponseTime { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }

    public record CreateApiStatusResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }
}