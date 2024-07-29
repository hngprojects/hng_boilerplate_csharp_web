using Google.Apis.Auth;
using Hng.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken)
        {
            return GoogleJsonWebSignature.ValidateAsync(idToken);
        }
    }
}
