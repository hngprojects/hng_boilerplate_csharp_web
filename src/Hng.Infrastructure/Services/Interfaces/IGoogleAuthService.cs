using Google.Apis.Auth;


namespace Hng.Infrastructure.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken);
    }
}
