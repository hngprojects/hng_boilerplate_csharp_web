using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class SubscriptionsTransactionCommandHandler : IRequestHandler<SubTransactionWebhookCommand, bool>
    {
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IRepository<Subscription> _subscriptionRepo;

        public SubscriptionsTransactionCommandHandler(
            IRepository<Transaction> transactionRepo,
            IRepository<Subscription> subscriptionRepo)
        {
            _transactionRepo = transactionRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        public async Task<bool> Handle(SubTransactionWebhookCommand request, CancellationToken cancellationToken)
        {
            if (!request.Command.Event.Equals(PaystackEventKeys.charge_success, StringComparison.OrdinalIgnoreCase))
                return false;

            try
            {
                var subInitialized = JsonConvert.DeserializeObject<SubscriptionInitialized>(JsonConvert.SerializeObject(request.Command.Data.Metadata));

                var transaction = await _transactionRepo.GetBySpec(r => r.Reference == request.Command.Data.Reference && r.ProductId == subInitialized.SubId);

                if (transaction == null)
                    return false;

                UpdateTransaction(transaction, request);

                await _transactionRepo.UpdateAsync(transaction);
                await _transactionRepo.SaveChanges();

                var subscription = await _subscriptionRepo.GetBySpec(r => r.Id == subInitialized.SubId);

                if (subscription == null)
                    return false;

                UpdateSubscription(subscription, transaction.Id);

                await _subscriptionRepo.UpdateAsync(subscription);
                await _subscriptionRepo.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateTransaction(Transaction transaction, SubTransactionWebhookCommand request)
        {
            transaction.Status = Domain.Enums.TransactionStatus.Completed;
            transaction.PaidAt = Convert.ToDateTime(request.Command.Data?.PaidAt).ToUniversalTime();
            transaction.ModifiedAt = DateTime.UtcNow;
        }

        public static void UpdateSubscription(Subscription subscription, Guid transactionId)
        {
            subscription.UpdatedAt = DateTime.UtcNow;
            subscription.TransactionId = transactionId;
            subscription.StartDate = DateTime.UtcNow;
            subscription.IsActive = true;
            subscription.ExpiryDate = BuildExpiryDate(subscription.Frequency, subscription.StartDate.GetValueOrDefault());
        }

        public static DateTime BuildExpiryDate(SubscriptionFrequency frequency, DateTime startDate)
        {
            var expiryDate = startDate;
            switch (frequency)
            {
                case SubscriptionFrequency.Monthly:
                    expiryDate.AddMonths(1);
                    break;
                case SubscriptionFrequency.Quarterly:
                    expiryDate.AddMonths(3);
                    break;
                case SubscriptionFrequency.HalfYearly:
                    expiryDate.AddMonths(6);
                    break;
                case SubscriptionFrequency.Annually:
                    expiryDate.AddYears(1);
                    break;
                default:
                    break;
            }
            return expiryDate;
        }
    }
}