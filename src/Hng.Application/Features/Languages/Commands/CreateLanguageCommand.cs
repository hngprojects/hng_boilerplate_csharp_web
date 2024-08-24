using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Languages.Commands
{
    public class CreateLanguageCommand : IRequest<SuccessResponseDto<LanguageDto>>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}