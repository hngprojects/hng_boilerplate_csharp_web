using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Application.Utils;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class InitiateSubscriptionTransactionHandler : IRequestHandler<InitiateSubscriptionTransactionCommand, Result<InitiateSubscriptionTransactionResponse>>
    {
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Subscription> _subscriptionRepo;
        private readonly IPaystackClient _paystackClient;
        private readonly PaystackApiKeys _apiKeys;

        public InitiateSubscriptionTransactionHandler(
            IRepository<Transaction> transactionRepo,
            IRepository<User> userRepo,
            IRepository<Subscription> subscriptionRepo,
            IPaystackClient paystackClient,
            PaystackApiKeys apiKeys)
        {
            _transactionRepo = transactionRepo;
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;
            _paystackClient = paystackClient;
            _apiKeys = apiKeys;
        }

        public async Task<Result<InitiateSubscriptionTransactionResponse>> Handle(InitiateSubscriptionTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepo.GetBySpec(u => u.Email == request.Email);

                if (user == null)
                    return Result.Failure<InitiateSubscriptionTransactionResponse>("User does not exist!");

                if (request.Amount <= 0)
                    return Result.Failure<InitiateSubscriptionTransactionResponse>("Amount must be greater than Zero!");

                var subscription = BuildSubscription(request, user.Id);

                await _subscriptionRepo.AddAsync(subscription);
                await _subscriptionRepo.SaveChanges();

                var amountInKobo = request.Amount * 100;
                var reference = GenerateTransactionReference.GenerateReference();
                var initializeRequest = new InitializeTransactionRequest(amountInKobo.ToString(), request.Email, reference)
                {
                    BusinessAuthorizationToken = _apiKeys.SecretKey,
                    Metadata = JsonConvert.SerializeObject(new SubscriptionInitialized(subscription.Id))
                };

                var result = await _paystackClient.InitializeTransaction(initializeRequest);

                if (result.IsSuccess && result.Value.Status && result.Value.Data != null)
                {
                    var transaction = BuildTransaction(request, reference, user.Id, subscription.Id);
                    await _transactionRepo.AddAsync(transaction);
                    await _transactionRepo.SaveChanges();

                    return Result.Success(new InitiateSubscriptionTransactionResponse()
                    {
                        Status = result.Value.Status,
                        Data = result.Value.Data,
                        Message = result.Value.Message
                    });
                }
                else
                {
                    return Result.Failure<InitiateSubscriptionTransactionResponse>(result.Error ?? "Invalid input parameters. Please check your request and try again.");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<InitiateSubscriptionTransactionResponse>($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static Transaction BuildTransaction(InitiateSubscriptionTransactionCommand request, string reference, Guid userId, Guid subId)
            => new Transaction()
            {
                Amount = request.Amount,
                Reference = reference,
                CreatedAt = DateTime.UtcNow,
                Partners = TransactionIntegrationPartners.Paystack,
                Status = TransactionStatus.Pending,
                UserId = userId,
                Type = TransactionType.subscription,
                SubscriptionId = subId
            };

        private static Subscription BuildSubscription(InitiateSubscriptionTransactionCommand request, Guid userId)
        {
            Enum.TryParse(request.Frequency, out SubscriptionFrequency frequency);
            Enum.TryParse(request.Plan, out SubscriptionPlan plan);

            return new Subscription()
            {
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                Frequency = frequency,
                IsActive = false,
                Plan = plan,
                UserId = userId
            };
        }
    }
}
