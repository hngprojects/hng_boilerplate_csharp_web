using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class FacebookLoginRequestDto
    {
        [Required]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
