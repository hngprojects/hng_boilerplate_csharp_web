using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public class GetTransactionsByUserIdQuery : IRequest<PagedListDto<TransactionDto>>
    {
        public Guid UserId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetTransactionsByUserIdQuery(Guid userId, int pageNumber, int pageSize)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}