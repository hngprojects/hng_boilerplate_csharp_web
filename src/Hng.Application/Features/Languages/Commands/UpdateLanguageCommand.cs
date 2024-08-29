using Hng.Application.Features.Languages.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Languages.Commands
{
    public class UpdateLanguageCommand : IRequest<LanguageResponseDto>
    {
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}