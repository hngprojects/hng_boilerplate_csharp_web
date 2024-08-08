using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public class GetTransactionsByProductIdQuery : IRequest<PagedListDto<TransactionDto>>
    {
        public Guid ProductId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetTransactionsByProductIdQuery(Guid productId, int pageNumber, int pageSize)
        {
            ProductId = productId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}