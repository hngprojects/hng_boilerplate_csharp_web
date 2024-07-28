using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class ProductsTransactionCommandHandler : IRequestHandler<TransactionsWebhookCommand, bool>
    {
        private readonly IRepository<Transaction> _paymentRepo;

        public ProductsTransactionCommandHandler(IRepository<Transaction> paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<bool> Handle(TransactionsWebhookCommand request, CancellationToken cancellationToken)
        {
            if (!request.Event.Equals(PaystackEventKeys.charge_success, StringComparison.OrdinalIgnoreCase))
                return false;

            try
            {
                if (!request.Data.Metadata.ToString().Contains(nameof(ProductInitialized.ProductId)))
                    return false;

                var productInitialized = 
                    JsonConvert.DeserializeObject<ProductInitialized>(JsonConvert.SerializeObject(request.Data.Metadata));

                var transaction = await _paymentRepo.GetBySpec(r => r.Reference == request.Data.Reference && r.ProductId == productInitialized.ProductId);

                if (transaction == null)
                    return false;

                transaction.Status = Domain.Enums.TransactionStatus.Completed;
                transaction.PaidAt = Convert.ToDateTime(request.Data?.PaidAt);
                transaction.ModifiedAt = DateTime.UtcNow;

                await _paymentRepo.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
}