using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface ISubscriptionPlanRepository
    {
        Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan);
        Task<bool> ExistsAsync(string name);
    }
}
