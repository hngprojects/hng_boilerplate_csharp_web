using Hng.Application.Dto;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanDto dto);
    }
}
