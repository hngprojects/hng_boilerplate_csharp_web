using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionPlanService> _logger;

        public SubscriptionPlanService(ISubscriptionPlanRepository repository, IMapper mapper, ILogger<SubscriptionPlanService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanDto dto)
        {
            _logger.LogInformation("Attempting to create new subscription plan");

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning("Invalid subscription plan name");
                throw new ArgumentException("Subscription plan name is required.");
            }

            if (await _repository.ExistsAsync(dto.Name))
            {
                _logger.LogWarning($"Subscription plan '{dto.Name}' already exists");
                throw new InvalidOperationException("Subscription plan already exists.");
            }

            var plan = _mapper.Map<SubscriptionPlan>(dto);
            var createdPlan = await _repository.CreateAsync(plan);
            _logger.LogInformation($"Subscription plan created successfully: {createdPlan.Id}");

            return _mapper.Map<SubscriptionPlanResponse>(createdPlan);
        }
    }
}
