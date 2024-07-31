using Hng.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        public async Task<FacebookUser> ValidateAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var userInfoResponse = await httpClient.GetAsync($"https://graph.facebook.com/me?fields=id,name,email,picture&access_token={accessToken}");
            if (!userInfoResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var userJson = await userInfoResponse.Content.ReadAsStringAsync();
            var facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userJson);

            return facebookUser;
        }
    }
}
