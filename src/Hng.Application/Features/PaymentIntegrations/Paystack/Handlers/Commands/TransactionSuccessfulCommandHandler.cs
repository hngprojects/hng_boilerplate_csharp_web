using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands
{
    public class TransactionSuccessfulCommandHandler : IRequestHandler<TransactionSuccessfulCommand, Result<string>>
    {
        private readonly IRepository<Transaction> _paymentRepo;
        public TransactionSuccessfulCommandHandler(IRepository<Transaction> paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<Result<string>> Handle(TransactionSuccessfulCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _paymentRepo.GetBySpec(r => r.Reference == request.Data.Reference);

            if (transaction == null)
                return Result.Failure<string>("Transaction not found");

            transaction.Status = Domain.Enums.TransactionStatus.Completed;
            transaction.PaidAt = Convert.ToDateTime(request.Data?.PaidAt);
            transaction.ModifiedAt = DateTime.UtcNow;

            await _paymentRepo.SaveChanges();

            return Result.Success("success");
        }
    }
}