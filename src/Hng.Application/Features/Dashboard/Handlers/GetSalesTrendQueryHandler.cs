using AutoMapper;
using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Dashboard.Handlers
{
    public class GetSalesTrendQueryHandler : IRequestHandler<GetSalesTrendQuery, PagedListDto<TransactionDto>>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetSalesTrendQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<PagedListDto<TransactionDto>> Handle(GetSalesTrendQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            if (userId == Guid.Empty)
            {
                throw new ApplicationException("User ID is not available in the claims.");
            }

            if (request.productsQueryParameters.StartDate == default(DateTime) || request.productsQueryParameters.EndDate == default(DateTime))
            {
                return null;
            }
            var transactions = await _transactionRepository.GetAllBySpec(x =>
            x.Status == Domain.Enums.TransactionStatus.Completed
            && x.CreatedAt >= request.productsQueryParameters.StartDate && x.CreatedAt <= request.productsQueryParameters.EndDate);

            if (transactions.Count() == 0) return null;

            var mappedSales = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            var productsResult = PagedListDto<TransactionDto>.ToPagedList(mappedSales, 1, 20);
            return productsResult;
        }
    }
}
