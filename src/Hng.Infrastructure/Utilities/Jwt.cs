namespace Hng.Infrastructure.Utilities
{
    public class Jwt
    {
        public string SecretKey { get; set; }

        public int ExpireInMinute { get; set; }
    }
}
