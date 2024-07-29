using CSharpFunctionalExtensions;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Handlers.Commands
{
    public class CreateFreeSubscriptionPlanHandler : IRequestHandler<SubscribeFreePlan, Result<SubscribeFreePlanResponse>>
    {
        private readonly IRepository<Subscription> _subscriptionRepo;

        public CreateFreeSubscriptionPlanHandler(IRepository<Subscription> subscriptionRepo)
        {
            _subscriptionRepo = subscriptionRepo;
        }

        public async Task<Result<SubscribeFreePlanResponse>> Handle(SubscribeFreePlan request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) && string.IsNullOrWhiteSpace(request.OrganizationId))
                return Result.Failure<SubscribeFreePlanResponse>("Both UserId and OrganizationId cannot be null");

            if (!string.IsNullOrWhiteSpace(request.UserId) && !string.IsNullOrWhiteSpace(request.OrganizationId))
                return Result.Failure<SubscribeFreePlanResponse>("Both UserId and OrganizationId cannot have values the same time");

            var subscription = BuildSubscription(request.UserId, request.OrganizationId);

            await _subscriptionRepo.AddAsync(subscription);
            await _subscriptionRepo.SaveChanges();

            return Result.Success(new SubscribeFreePlanResponse()
            {
                Frequency = subscription.Frequency.ToString(),
                IsActive = subscription.IsActive,
                OrganizationId = subscription.OrganizationId,
                Plan = subscription.Plan.ToString(),
                StartDate = DateTime.Now,
                UserId = subscription.UserId,
                SubscriptionId = subscription.Id
            });

        }

        private static Subscription BuildSubscription(string userId, string organisationId)
            => new Subscription()
            {
                CreatedAt = DateTime.UtcNow,
                Frequency = SubscriptionFrequency.Annually,
                IsActive = true,
                Plan = SubscriptionPlan.Free,
                StartDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                UserId = string.IsNullOrWhiteSpace(userId) ? null : Guid.Parse(userId),
                OrganizationId = string.IsNullOrWhiteSpace(organisationId) ? null : Guid.Parse(organisationId),
                Amount = 0
            };
    }
}
