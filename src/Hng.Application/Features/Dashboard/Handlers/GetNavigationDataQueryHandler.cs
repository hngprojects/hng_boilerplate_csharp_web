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
    public class GetNavigationDataQueryHandler : IRequestHandler<GetNavigationDataQuery, NavigationDataDto>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetNavigationDataQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<NavigationDataDto> Handle(GetNavigationDataQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            if (userId == Guid.Empty)
            {
                throw new ApplicationException("User ID is not available in the claims.");
            }

            var transactions = await _transactionRepository.GetAllBySpec(x =>
            x.Status == Domain.Enums.TransactionStatus.Completed && x.CreatedAt.Year == DateTime.Now.Year);

            if (transactions.Count() == 0) return null;

			List<ReportDataDto> result = transactions
	            .GroupBy(l => l.CreatedAt.Month)
	            .Select(cl => new ReportDataDto
				{
		            Month = cl.First().CreatedAt.Month,
		            Revenue = cl.Sum(c => c.Amount),
	            }).ToList();

            return new NavigationDataDto { Overview = result};
        }
    }
}
