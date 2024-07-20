

namespace Hng.Domain.Entities.Models
{
    public class User :BaseModel
    {

        public required string Email { get; set; }
        public required string UserName { get; set; }
        public string? Password { get; set; }

        public string? RefreshToken { get; set; }
    }
}
