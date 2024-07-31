using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<FacebookUser> ValidateAsync(string accessToken);
    }

    public class FacebookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public FacebookPictureData Picture { get; set; }
    }

    public class FacebookPictureData
    {
        public FacebookPicture Data { get; set; }
    }

    public class FacebookPicture
    {
        public string Url { get; set; }
    }
}
