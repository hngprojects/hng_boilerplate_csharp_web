using AutoMapper;
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
        private readonly IRepository<Transaction> _paymentRepo;
        private readonly IMapper _mapper;
        private readonly IPaystackClient _paystackClient;
        private readonly PaystackApiKeys _apiKeys;

        public InitializeTransactionCommandHandler(IPaystackClient paystackClient, PaystackApiKeys apiKeys, IRepository<Transaction> paymentRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _paystackClient = paystackClient;
            _apiKeys = apiKeys;
            _mapper = mapper;

        }

        public async Task<Result<InitializeTransactionResponse>> Handle(InitializeTransactionCommand request, CancellationToken cancellationToken)
        {
            var amountInKobo = request.Amount * 100;
            var reference = GenerateReference();
            var initializeRequest = new InitializeTransactionRequest(amountInKobo.ToString(), request.Email)
            {
                BusinessAuthorizationToken = _apiKeys.SecretKey,
                Reference = reference,
                Metadata = JsonConvert.SerializeObject(new
                {
                    product_id = request.ProductId
                })
            };
            try
            {
                var result = await _paystackClient.InitializeTransaction(initializeRequest);

                if (result.IsSuccess && result.Value.Data != null)
                {
                    result.Value.Data.Amount = amountInKobo / 100m;

                    var transaction = _mapper.Map<Transaction>(request);
                    transaction.Id = Guid.NewGuid();
                    transaction.Amount = result.Value.Data.Amount;
                    transaction.Reference = reference;
                    transaction.Status = TransactionStatus.Pending;
                    transaction.CreatedAt = DateTime.UtcNow;
                    transaction.ProductId = request.ProductId;

                    await _paymentRepo.AddAsync(transaction);
                    await _paymentRepo.SaveChanges();

                    return Result.Success(new InitializeTransactionResponse
                    {
                        Status = result.Value.Status,
                        Message = "Transaction initialized",
                        Data = result.Value.Data
                    });
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

    }
}
