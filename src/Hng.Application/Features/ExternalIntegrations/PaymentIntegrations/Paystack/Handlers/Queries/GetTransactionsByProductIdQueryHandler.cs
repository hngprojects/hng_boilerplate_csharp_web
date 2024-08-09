using AutoMapper;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Queries
{
    public class GetTransactionsByProductIdQueryHandler : IRequestHandler<GetTransactionsByProductIdQuery, PagedListDto<TransactionDto>>
    {
        private readonly IRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public GetTransactionsByProductIdQueryHandler(IRepository<Transaction> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<TransactionDto>> Handle(GetTransactionsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetAllBySpec(t => t.ProductId == request.ProductId);
            var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return PagedListDto<TransactionDto>.ToPagedList(transactionDtos, request.PageNumber, request.PageSize);
        }
    }
}
