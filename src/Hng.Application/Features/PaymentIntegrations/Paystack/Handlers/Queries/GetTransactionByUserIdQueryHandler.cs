using AutoMapper;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Queries
{
    public class GetTransactionsByUserIdQueryHandler : IRequestHandler<GetTransactionsByUserIdQuery, PagedListDto<TransactionDto>>
    {
        private readonly IRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public GetTransactionsByUserIdQueryHandler(IRepository<Transaction> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<TransactionDto>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetAllBySpec(t => t.UserId == request.UserId);
            var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return PagedListDto<TransactionDto>.ToPagedList(transactionDtos, request.PageNumber, request.PageSize);
        }
    }
}