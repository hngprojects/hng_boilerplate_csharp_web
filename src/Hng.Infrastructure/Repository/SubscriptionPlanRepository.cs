using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly MyDBContext _context;

        public SubscriptionPlanRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.SubscriptionPlans.AnyAsync(p => p.Name == name);
        }
    }
}
