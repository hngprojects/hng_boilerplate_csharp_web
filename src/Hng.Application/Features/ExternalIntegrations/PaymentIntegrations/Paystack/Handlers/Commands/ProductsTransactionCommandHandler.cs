using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class ProductsTransactionCommandHandler : IRequestHandler<TransactionWebhookCommand, bool>
    {
        private readonly IRepository<Transaction> _transactionrepo;

        public ProductsTransactionCommandHandler(IRepository<Transaction> paymentRepo)
        {
            _transactionrepo = paymentRepo;
        }

        public async Task<bool> Handle(TransactionWebhookCommand request, CancellationToken cancellationToken)
        {
            if (!request.Command.Event.Equals(PaystackEventKeys.charge_success, StringComparison.OrdinalIgnoreCase))
                return false;

            try
            {
                var productInitialized = JsonConvert.DeserializeObject<ProductInitialized>(JsonConvert.SerializeObject(request.Command.Data.Metadata));

                var transaction = await _transactionrepo.GetBySpec(r => r.Reference == request.Command.Data.Reference && r.ProductId == productInitialized.ProductId);

                if (transaction == null)
                    return false;

                transaction.Status = Domain.Enums.TransactionStatus.Completed;
                transaction.PaidAt = Convert.ToDateTime(request.Command.Data?.PaidAt).ToUniversalTime();
                transaction.ModifiedAt = DateTime.UtcNow;

                await _transactionrepo.UpdateAsync(transaction);
                await _transactionrepo.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}