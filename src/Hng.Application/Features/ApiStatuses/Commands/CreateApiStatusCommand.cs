using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.ApiStatuses.Commands
{
    public class CreateApiStatusCommand : IRequest<CreateApiStatusResponseDto>
    {
        [Required]
        public IFormFile File { get; set; }
    }
}