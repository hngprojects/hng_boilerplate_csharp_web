using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class InitializeTransactionCommandHandler : IRequestHandler<InitializeTransactionCommand, Result<InitializeTransactionResponse>>
    {
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IPaystackClient _paystackClient;
        private readonly PaystackApiKeys _apiKeys;

        public InitializeTransactionCommandHandler(
            IPaystackClient paystackClient,
            PaystackApiKeys apiKeys,
            IRepository<Transaction> transactionRepo,
            IRepository<User> userRepo,
            IRepository<Product> productRepo)
        {
            _transactionRepo = transactionRepo;
            _paystackClient = paystackClient;
            _apiKeys = apiKeys;
            _userRepo = userRepo;
            _productRepo = productRepo;
        }

        public async Task<Result<InitializeTransactionResponse>> Handle(InitializeTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepo.GetBySpec(u => u.Email == request.Email);

                if (user == null) 
                    return Result.Failure<InitializeTransactionResponse>("User does not exist!");

                var product = await _productRepo.GetBySpec(p => p.Id == request.ProductId);

                if(product == null)
                    return Result.Failure<InitializeTransactionResponse>("Product with not found!");

                var amountInKobo = request.Amount * 100;
                var reference = GenerateReference();
                var initializeRequest = new InitializeTransactionRequest(amountInKobo.ToString(), request.Email)
                {
                    BusinessAuthorizationToken = _apiKeys.SecretKey,
                    Reference = reference,
                    Metadata = JsonConvert.SerializeObject(new ProductInitialized(request.ProductId))
                };

                var result = await _paystackClient.InitializeTransaction(initializeRequest);

                if (result.IsSuccess && result.Value.Status && result.Value.Data != null)
                {
                    var transaction = BuildTransaction(request, reference, user.Id);
                    
                    await _transactionRepo.AddAsync(transaction);
                    await _transactionRepo.SaveChanges();

                    return Result.Success(result.Value);
                }
                else
                {
                    return Result.Failure<InitializeTransactionResponse>(result.Error ?? "Invalid input parameters. Please check your request and try again.");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<InitializeTransactionResponse>($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static string GenerateReference() => $"hng{DateTime.Now.Ticks}";

        private static Transaction BuildTransaction(InitializeTransactionCommand request, string reference, Guid userId)
            => new Transaction()
            {
                Amount = request.Amount,
                Reference = reference,
                CreatedAt = DateTime.UtcNow,
                Partners = TransactionIntegrationPartners.Paystack,
                ProductId = request.ProductId,
                Status = TransactionStatus.Pending,
                UserId = userId,
                Type = TransactionType.product
            };
    }
}
