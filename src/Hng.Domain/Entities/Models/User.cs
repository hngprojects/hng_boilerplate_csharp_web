
using Microsoft.AspNetCore.Identity;

namespace Hng.Domain.Entities.Models
{
    public class User : IdentityUser<long>
    {

        public string? Password { get; set; }

        public string? RefreshToken { get; set; }
    }
}
