using AutoMapper;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Dashboard.Handlers
{
    public class GetExportDataQueryHandler : IRequestHandler<GetExportDataQuery, List<TransactionDto>>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetExportDataQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<List<TransactionDto>> Handle(GetExportDataQuery request, CancellationToken cancellationToken)
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

            var transactions = await _transactionRepository.GetAllBySpec(x => x.CreatedAt >= request.productsQueryParameters.StartDate && x.CreatedAt <= request.productsQueryParameters.EndDate);

            if (transactions.Count() == 0) return null;

            var response = _mapper.Map<List<TransactionDto>>(transactions);
            return response;
        }
    }
}
