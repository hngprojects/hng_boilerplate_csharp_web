using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class GoogleLoginRequestDto
    {
        [Required]
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }
}
