using Hng.Application.Models.WaitlistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public interface IWaitlistService
    {
        Task<WaitlistUserResponseModel> SignUpAsync(WaitlistUserRequestModel model);
    }
}
